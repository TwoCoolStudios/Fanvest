using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fanvest.Core.Models.Users;
namespace Fanvest.Services.Users
{
    public partial interface IUserService
    {
        #region Users
        Task DeleteUser(User user);

        Task<IList<User>> GetAllUsers();

        Task<User> GetUserById(int userId);

        Task<User> GetUserByEmail(string email);

        Task<User> GetUserByGuid(Guid userGuid);

        Task<User> GetUserByUsername(string username);

        Task InsertUser(User user);

        Task UpdateUser(User user);

        Task<User> InsertVisitor();

        string GetUserFullName(User user);

        Task<string> FormatUserName(User user,
            bool stripTooLong = false, int maxLength = 0);
        #endregion

        #region Roles
        Task DeleteRole(Role role);

        Task<IList<Role>> GetAllRoles();

        Task<Role> GetRoleById(int roleId);

        Task<Role> GetRoleBySystemName(string systemName);

        Task InsertRole(Role role);

        Task UpdateRole(Role role);

        Task<bool> IsInRole(User user, string systemName);
        #endregion

        #region Passwords
        Task<IList<UserPassword>> GetUserPasswords(int? userId = null,
            PasswordFormat? passwordFormat = null, int? passwordsToReturn = null);

        Task<UserPassword> GetCurrentPassword(int userId);

        Task InsertUserPassword(UserPassword password);

        Task UpdateUserPassword(UserPassword password);

        bool IsPasswordRecoveryTokenValid(User user, string token);

        bool HasPasswordRecoveryLinkExpired(User user);
        #endregion

        #region Transactions
        Task<IList<Transaction>> GetAllTransactions(int userId);

        Task InsertTransaction(Transaction transaction);
        #endregion
    }
}