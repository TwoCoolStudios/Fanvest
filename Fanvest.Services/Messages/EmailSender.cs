using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
namespace Fanvest.Services.Messages
{
    public partial class EmailSender : IEmailSender
    {
        public virtual async Task SendEmail(string emailAccount, string subject,
            string body, string fromAddress, string toAddress)
        {
            var message = new MailMessage
            {
                From = new MailAddress(fromAddress)
            };
            message.To.Add(new MailAddress(toAddress));
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Host = "mail.fanvest.io";
                smtpClient.Port = 25;
                smtpClient.Credentials = false ? CredentialCache.DefaultNetworkCredentials
                    : new NetworkCredential("support@fanvest.io", "");
                await smtpClient.SendMailAsync(message);
            }
        }
    }
}
