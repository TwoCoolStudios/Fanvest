using System;
using System.Linq;
using System.Threading.Tasks;
using Fanvest.Core;
using Fanvest.Core.Models;
using Fanvest.Core.Models.Users;
using Fanvest.Services.Security;
using Microsoft.Extensions.Options;
namespace Fanvest.Services.Users
{
    public partial class UserRegistrationService : IUserRegistrationService
    {
        #region Fields
        private readonly IEncryptionService _encryptionService;
        private readonly IUserService _userService;
        private readonly SiteSettings _siteSettings;
        #endregion

        #region Ctors
        public UserRegistrationService(IEncryptionService encryptionService,
            IUserService userService, IOptions<SiteSettings> siteSettings)
        {
            _encryptionService = encryptionService;
            _userService = userService;
            _siteSettings = siteSettings.Value;
        }
        #endregion

        #region Utilities
        protected bool PasswordsMatch(UserPassword userPassword, string enteredPassword)
        {
            if (userPassword == null || string.IsNullOrEmpty(enteredPassword))
                return false;
            var savedPassword = string.Empty;
            switch (userPassword.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    savedPassword = enteredPassword;
                    break;
                case PasswordFormat.Hashed:
                    savedPassword = _encryptionService.CreatePasswordHash(enteredPassword,
                        userPassword.SaltKey, _siteSettings.HashedPasswordFormat);
                    break;
                case PasswordFormat.Encrypted:
                    savedPassword = _encryptionService.EncryptText(enteredPassword);
                    break;
            }
            if (userPassword.Password == null)
                return false;
            return userPassword.Password.Equals(savedPassword);
        }
        #endregion

        #region Methods
        public virtual async Task<UserLoginResult> ValidateUser(string usernameOrEmail, string password)
        {
            var user = _siteSettings.UsernamesEnabled
                ? await _userService.GetUserByUsername(usernameOrEmail)
                : await _userService.GetUserByEmail(usernameOrEmail);
            if (user == null)
                return UserLoginResult.UserDoesntExist;
            if (user.Deleted)
                return UserLoginResult.Deleted;
            if (!user.Active)
                return UserLoginResult.NotActive;
            if (!await _userService.IsInRole(user, UserDefaults.Registered))
                return UserLoginResult.NotRegistered;
            if (user.CannotLoginUntilDate.HasValue && user.CannotLoginUntilDate.Value > DateTime.Now)
                return UserLoginResult.LockedOut;
            if (!PasswordsMatch(await _userService.GetCurrentPassword(user.Id), password))
            {
                user.FailedLoginAttempts++;
                if (_siteSettings.FailedPasswordAttemptsAllowed > 0
                    && user.FailedLoginAttempts >= _siteSettings.FailedPasswordAttemptsAllowed)
                {
                    user.CannotLoginUntilDate = DateTime.Now.AddMinutes(_siteSettings.FailedPasswordLockOutMinutes);
                    user.FailedLoginAttempts = 0;
                }
                await _userService.UpdateUser(user);
                return UserLoginResult.WrongPassword;
            }
            user.FailedLoginAttempts = 0;
            user.CannotLoginUntilDate = null;
            user.LastLoginDate = DateTime.Now;
            await _userService.UpdateUser(user);
            return UserLoginResult.Successful;
        }

        public virtual async Task<UserRegistrationResult> RegisterUser(UserRegistrationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (request.User == null)
                throw new ArgumentException("Could not load current user.");
            var result = new UserRegistrationResult();
            if (await _userService.IsInRole(request.User, UserDefaults.Registered))
            {
                result.AddError("Current user has already been registered.");
                return result;
            }
            if (string.IsNullOrEmpty(request.Email))
            {
                result.AddError("Email address was not provided.");
                return result;
            }
            if (!CommonHelper.IsValidEmail(request.Email))
            {
                result.AddError("Wrong email address format.");
                return result;
            }
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                result.AddError("Password was not provided.");
                return result;
            }
            if (_siteSettings.UsernamesEnabled && string.IsNullOrEmpty(request.Username))
            {
                result.AddError("Username was not provided.");
                return result;
            }
            if (await _userService.GetUserByEmail(request.Email) != null)
            {
                result.AddError("Email address already exists.");
                return result;
            }
            if (_siteSettings.UsernamesEnabled && await _userService.GetUserByUsername(request.Username) != null)
            {
                result.AddError("Username already exists.");
                return result;
            }
            request.User.Email = request.Email;
            request.User.Username = request.Username;
            var userPassword = new UserPassword
            {
                User = request.User,
                PasswordFormat = request.PasswordFormat,
                CreatedOn = DateTime.Now
            };
            switch (request.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    userPassword.Password = request.Password;
                    break;
                case PasswordFormat.Hashed:
                    {
                        var saltKey = _encryptionService.CreateSaltKey(UserDefaults.PasswordSaltKeySize);
                        userPassword.SaltKey = saltKey;
                        userPassword.Password = _encryptionService.CreatePasswordHash
                            (request.Password, saltKey, _siteSettings.HashedPasswordFormat);
                    }
                    break;
                case PasswordFormat.Encrypted:
                    userPassword.Password = _encryptionService.EncryptText(request.Password);
                    break;
            }
            await _userService.InsertUserPassword(userPassword);
            request.User.Active = request.Approved;
            var registeredRole = await _userService.GetRoleBySystemName(UserDefaults.Registered);
            if (registeredRole == null)
                throw new CustomException("'Registered' role could not be loaded.");
            request.User.AddUserToRole(new UserRole { Role = registeredRole });
            var visitorRole = request.User.Roles.FirstOrDefault(r => r.SystemName == UserDefaults.Visitors);
            if (visitorRole != null)
                request.User.RemoveUserFromRole(request.User.UserRoles.FirstOrDefault(ur => ur.RoleId == visitorRole.Id));
            await _userService.UpdateUser(request.User);
            return result;
        }

        public virtual async Task<ChangePasswordResult> ChangePassword(ChangePasswordRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            var result = new ChangePasswordResult();
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                result.AddError("Email address was not provided.");
                return result;
            }
            if (string.IsNullOrWhiteSpace(request.NewPassword))
            {
                result.AddError("Password was not provided.");
                return result;
            }
            var user = await _userService.GetUserByEmail(request.Email);
            if (user == null)
            {
                result.AddError("Email address could not be found.");
                return result;
            }
            if (request.ValidateRequest)
            {
                if (!PasswordsMatch(await _userService.GetCurrentPassword(user.Id), request.OldPassword))
                {
                    result.AddError("Entered passwords do not match.");
                    return result;
                }
            }
            var userPassword = new UserPassword
            {
                User = user,
                PasswordFormat = request.NewPasswordFormat,
                CreatedOn = DateTime.Now
            };
            switch (request.NewPasswordFormat)
            {
                case PasswordFormat.Clear:
                    userPassword.Password = request.NewPassword;
                    break;
                case PasswordFormat.Hashed:
                    {
                        var saltKey = _encryptionService.CreateSaltKey(UserDefaults.PasswordSaltKeySize);
                        userPassword.SaltKey = saltKey;
                        userPassword.Password = _encryptionService.CreatePasswordHash
                            (request.NewPassword, saltKey, _siteSettings.HashedPasswordFormat);
                    }
                    break;
                case PasswordFormat.Encrypted:
                    userPassword.Password = _encryptionService.EncryptText(request.NewPassword);
                    break;
            }
            await _userService.InsertUserPassword(userPassword);
            return result;
        }

        public virtual async Task SetEmail(User user, string newEmail)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (newEmail == null)
                throw new CustomException("Email address cannot be null.");
            newEmail = newEmail.Trim();
            var oldEmail = user.Email;
            if (!CommonHelper.IsValidEmail(newEmail))
                throw new CustomException("Wrong email address format.");
            if (newEmail.Length > 200)
                throw new CustomException("Email address is too long.");
            var user2 = await _userService.GetUserByEmail(newEmail);
            if (user2 != null && user.Id != user2.Id)
                throw new CustomException("Email address already exists.");
            user.Email = newEmail;
            await _userService.UpdateUser(user);
            if (string.IsNullOrEmpty(oldEmail) || oldEmail.Equals(newEmail,
                    StringComparison.InvariantCultureIgnoreCase))
                return;
        }

        public virtual async Task SetUsername(User user, string newUsername)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (!_siteSettings.UsernamesEnabled)
                throw new CustomException("Usernames have been disabled.");
            newUsername = newUsername.Trim();
            if (newUsername.Length > 200)
                throw new CustomException("Username is too long.");
            var user2 = await _userService.GetUserByUsername(newUsername);
            if (user2 != null && user.Id != user2.Id)
                throw new CustomException("Username already exists.");
            user.Username = newUsername;
            await _userService.UpdateUser(user);
        }
        #endregion
    }
}