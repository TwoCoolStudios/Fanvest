using System.Threading.Tasks;
using Fanvest.Core.Models.Users;
namespace Fanvest.Core
{
    public partial interface IWorkContext
    {
        Task<User> GetCurrentUser();
    }
}