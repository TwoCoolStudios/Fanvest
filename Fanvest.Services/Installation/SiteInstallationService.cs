using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fanvest.Core.Models;
using Fanvest.Core.Models.Users;
using Fanvest.Data;
using Fanvest.Services.Security;
using Microsoft.Extensions.Options;
namespace Fanvest.Services.Installation
{
    public partial class SiteInstallationService : IInstallationService
    {
        #region Fields
        private readonly CustomDbContext _context;
        private readonly IEncryptionService _encryptionService;
        private readonly SiteSettings _siteSettings;
        #endregion

        #region Ctors
        public SiteInstallationService(CustomDbContext context,
            IEncryptionService encryptionService,
            IOptions<SiteSettings> siteSettings)
        {
            _context = context;
            _encryptionService = encryptionService;
            _siteSettings = siteSettings.Value;
        }
        #endregion

        #region Utilities
        protected virtual async Task CreateUsersAndRoles()
        {
            //roles
            var rAdministrators = new Role
            {
                Name = UserDefaults.Administrators,
                SystemRole = true,
                SystemName = UserDefaults.Administrators,
                Active = true
            };
            var rRegistered = new Role
            {
                Name = UserDefaults.Registered,
                SystemRole = true,
                SystemName = UserDefaults.Registered,
                Active = true
            };
            var rVisitors = new Role
            {
                Name = UserDefaults.Visitors,
                SystemRole = true,
                SystemName = UserDefaults.Visitors,
                Active = true
            };
            var rModerators = new Role
            {
                Name = UserDefaults.Moderators,
                SystemRole = true,
                SystemName = UserDefaults.Moderators,
                Active = true
            };
            var roles = new List<Role>
            {
                rAdministrators, rRegistered, rVisitors, rModerators
            };
            _context.Set<Role>().AddRange(roles);
            await _context.SaveChangesAsync();
            //users
            var adminUser = new User
            {
                UserGuid = Guid.NewGuid(),
                Email = "nruimveld@fanvest.io",
                Username = "nruimveld",
                FirstName = "Nick",
                LastName = "Ruimveld",
                Balance = 0M,
                TokenCount = 0,
                SystemAccount = true,
                Active = true,
                LastActivityDate = DateTime.Now,
                CreatedOn = DateTime.Now
            };
            adminUser.AddUserToRole(new UserRole { Role = rAdministrators });
            adminUser.AddUserToRole(new UserRole { Role = rRegistered });
            _context.Set<User>().Add(adminUser);
            await _context.SaveChangesAsync();
            //passwords
            _context.Set<UserPassword>().Add(new UserPassword
            {
                User = adminUser,
                PasswordFormat = PasswordFormat.Hashed,
                Password = _encryptionService.CreatePasswordHash("",
                    string.Empty, _siteSettings.HashedPasswordFormat),
                SaltKey = string.Empty,
                CreatedOn = DateTime.Now
            });
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Methods
        public virtual async Task CreateData()
        {
            await CreateUsersAndRoles();
        }
        #endregion
    }
}
