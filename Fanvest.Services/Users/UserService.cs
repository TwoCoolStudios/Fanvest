using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanvest.Core;
using Fanvest.Core.Models;
using Fanvest.Core.Models.Users;
using Fanvest.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
namespace Fanvest.Services.Users
{
    public partial class UserService : IUserService
    {
        #region Fields
        private readonly CustomDbContext _context;
        private readonly SiteSettings _siteSettings;
        #endregion

        #region Ctors
        public UserService(CustomDbContext context,
            IOptions<SiteSettings> siteSettings)
        {
            _context = context;
            _siteSettings = siteSettings.Value;
        }
        #endregion

        #region Methods
        #region Users
        public virtual async Task DeleteUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (user.SystemAccount)
                throw new CustomException($"System user ({user.SystemName}) cannot be deleted.");
            user.Deleted = true;
            if (_siteSettings.SuffixDeletedUsers)
            {
                if (!string.IsNullOrEmpty(user.Email))
                    user.Email += "-DELETED";
            }
            await UpdateUser(user);
        }

        public virtual async Task<IList<User>> GetAllUsers()
            => await _context.Set<User>().Where(u => !u.Deleted)
            .OrderByDescending(u => u.CreatedOn).Select(u => u)
            .ToListAsync();

        public virtual async Task<User> GetUserById(int userId)
        {
            if (userId == 0)
                return null;
            return await _context.Set<User>().FindAsync(userId);
        }

        public virtual async Task<User> GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;
            var query = from u in _context.Set<User>()
                        where u.Email == email
                        orderby u.Id
                        select u;
            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<User> GetUserByGuid(Guid userGuid)
        {
            if (userGuid == Guid.Empty)
                return null;
            var query = from u in _context.Set<User>()
                        where u.UserGuid == userGuid
                        orderby u.Id
                        select u;
            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<User> GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;
            var query = from u in _context.Set<User>()
                        where u.Username == username
                        orderby u.Id
                        select u;
            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task InsertUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            _context.Set<User>().Add(user);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            _context.Set<User>().Update(user);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<User> InsertVisitor()
        {
            var user = new User
            {
                UserGuid = Guid.NewGuid(),
                Email = "visitor@fanvest.io",
                FirstName = "Site",
                LastName = "Visitor",
                Active = true,
                LastActivityDate = DateTime.Now,
                CreatedOn = DateTime.Now
            };
            var visitorRole = await GetRoleBySystemName(UserDefaults.Visitors);
            if (visitorRole == null)
                throw new CustomException("'Visitors' role could not be loaded.");
            user.AddUserToRole(new UserRole { Role = visitorRole });
            _context.Set<User>().Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public virtual string GetUserFullName(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            var firstName = user.FirstName;
            var lastName = user.LastName;
            var fullName = string.Empty;
            if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
                fullName = $"{firstName} {lastName}";
            else
            {
                if (!string.IsNullOrWhiteSpace(firstName))
                    fullName = firstName;
                if (!string.IsNullOrWhiteSpace(lastName))
                    fullName = lastName;
            }
            return fullName;
        }

        public virtual async Task<string> FormatUserName(User user,
            bool stripTooLong = false, int maxLength = 0)
        {
            if (user == null)
                return string.Empty;
            if (await IsInRole(user, UserDefaults.Visitors))
                return "Visitor";
            var result = string.Empty;
            switch (_siteSettings.UserNameFormat)
            {
                case UserNameFormat.ShowEmails:
                    result = user.Email;
                    break;
                case UserNameFormat.ShowUsernames:
                    result = user.Email;
                    break;
                case UserNameFormat.ShowFullNames:
                    result = GetUserFullName(user);
                    break;
                case UserNameFormat.ShowFirstNames:
                    result = user.FirstName;
                    break;
                default:
                    break;
            }
            if (stripTooLong && maxLength > 0)
                result = CommonHelper.EnsureMaximumLength(result, maxLength);
            return result;
        }
        #endregion

        #region Roles
        public virtual async Task DeleteRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            if (role.SystemRole)
                throw new CustomException($"System role ({role.SystemName}) cannot be deleted.");
            _context.Set<Role>().Remove(role);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<IList<Role>> GetAllRoles()
        {
            var query = from r in _context.Set<Role>()
                        orderby r.Id
                        select r;
            return await query.ToListAsync();
        }

        public virtual async Task<Role> GetRoleById(int roleId)
        {
            if (roleId == 0)
                return null;
            return await _context.Set<Role>().FindAsync(roleId);
        }

        public virtual async Task<Role> GetRoleBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;
            var query = from r in _context.Set<Role>()
                        where r.SystemName == systemName
                        orderby r.Id
                        select r;
            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task InsertRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            _context.Set<Role>().Add(role);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            _context.Set<Role>().Update(role);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> IsInRole(User user, string systemName)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(systemName))
                throw new ArgumentNullException(nameof(systemName));
            return await _context.Set<UserRole>().FirstOrDefaultAsync(ur => ur.User == user
                && ur.Role.SystemName == systemName) != null;
        }
        #endregion

        #region Passwords
        public virtual async Task<IList<UserPassword>> GetUserPasswords(int? userId = null,
            PasswordFormat? passwordFormat = null, int? passwordsToReturn = null)
        {
            IQueryable<UserPassword> query = _context.Set<UserPassword>();
            if (userId.HasValue)
                query = query.Where(u => u.UserId == userId.Value);
            if (passwordFormat.HasValue)
                query = query.Where(u => u.PasswordFormatId == (int)passwordFormat.Value);
            if (passwordsToReturn.HasValue)
                query = query.OrderByDescending(p => p.CreatedOn).Take(passwordsToReturn.Value);
            return await query.ToListAsync();
        }

        public virtual async Task<UserPassword> GetCurrentPassword(int userId)
        {
            if (userId == 0)
                return null;
            var passwords = await GetUserPasswords(userId, passwordsToReturn: 1);
            return passwords.FirstOrDefault();
        }

        public virtual async Task InsertUserPassword(UserPassword password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            _context.Set<UserPassword>().Add(password);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateUserPassword(UserPassword password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            _context.Set<UserPassword>().Update(password);
            await _context.SaveChangesAsync();
        }

        public virtual bool IsPasswordRecoveryTokenValid(User user, string token)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            var uPrt = user.PasswordRecoveryToken;
            if (string.IsNullOrEmpty(uPrt))
                return false;
            if (!uPrt.Equals(token, StringComparison.InvariantCultureIgnoreCase))
                return false;
            return true;
        }

        public virtual bool HasPasswordRecoveryLinkExpired(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (_siteSettings.DaysPasswordRecoveryLinkValid == 0)
                return false;
            var generatedDate = user.PasswordRecoveryTokenDateGenerated;
            if (!generatedDate.HasValue)
                return false;
            var daysPassed = (DateTime.Now - generatedDate.Value).TotalDays;
            if (daysPassed > _siteSettings.DaysPasswordRecoveryLinkValid)
                return true;
            return false;
        }
        #endregion

        #region Transactions
        public virtual async Task<IList<Transaction>> GetAllTransactions(int userId)
        {
            if (userId == 0)
                return null;
            var query = from t in _context.Set<Transaction>()
                        where t.UserId == userId
                        orderby t.CreatedOn descending
                        select t;
            return await query.ToListAsync();
        }

        public virtual async Task InsertTransaction(Transaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));
            _context.Set<Transaction>().Add(transaction);
            await _context.SaveChangesAsync();
        }
        #endregion
        #endregion
    }
}