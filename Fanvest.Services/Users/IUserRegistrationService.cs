using System.Threading.Tasks;
using Fanvest.Core.Models.Users;
namespace Fanvest.Services.Users
{
    public partial interface IUserRegistrationService
    {
        Task<UserLoginResult> ValidateUser(string usernameOrEmail, string password);

        Task<UserRegistrationResult> RegisterUser(UserRegistrationRequest request);

        Task<ChangePasswordResult> ChangePassword(ChangePasswordRequest request);

        Task SetEmail(User user, string newEmail);

        Task SetUsername(User user, string newUsername);
    }
}