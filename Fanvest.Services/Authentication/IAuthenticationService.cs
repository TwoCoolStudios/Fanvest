using System.Threading.Tasks;
using Fanvest.Core.Models.Users;
namespace Fanvest.Services.Authentication
{
    public partial interface IAuthenticationService
    {
        Task SignIn(User user, bool isPersistent);

        Task SignOut();

        Task<User> GetAuthenticatedUser();
    }
}