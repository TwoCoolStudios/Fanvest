using System.Threading.Tasks;
namespace Fanvest.Services.Messages
{
    public partial interface IEmailSender
    {
        Task SendEmail(string emailAccount, string subject, string body,
            string fromAddress, string toAddress);
    }
}