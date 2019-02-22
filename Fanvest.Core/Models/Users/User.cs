using System;
using System.Collections.Generic;
using System.Linq;
namespace Fanvest.Core.Models.Users
{
    public partial class User : BaseEntity
    {
        public User() => UserGuid = Guid.NewGuid();

        public Guid UserGuid { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Balance { get; set; }
        public int TokenCount { get; set; }
        public string AccountActivationToken { get; set; }
        public string PasswordRecoveryToken { get; set; }
        public DateTime? PasswordRecoveryTokenDateGenerated { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? CannotLoginUntilDate { get; set; }
        public bool SystemAccount { get; set; }
        public string SystemName { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime LastActivityDate { get; set; }
        public DateTime CreatedOn { get; set; }

        private ICollection<Transaction> _transactions;
        public virtual ICollection<Transaction> Transactions
        {
            get => _transactions ?? (_transactions = new List<Transaction>());
            protected set => _transactions = value;
        }

        private ICollection<UserRole> _userRoles;
        public virtual ICollection<UserRole> UserRoles
        {
            get => _userRoles ?? (_userRoles = new List<UserRole>());
            protected set => _userRoles = value;
        }

        private IList<Role> _roles;
        public virtual IList<Role> Roles => _roles ??
            (_roles = UserRoles.Select(r => r.Role).ToList());

        public void AddUserToRole(UserRole userRole)
        {
            UserRoles.Add(userRole);
            _roles = null;
        }

        public void RemoveUserFromRole(UserRole userRole)
        {
            UserRoles.Remove(userRole);
            _roles = null;
        }
    }
}