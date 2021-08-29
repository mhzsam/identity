using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebApplication.Repository
{
    public class MessageSender:ImessageSender
    {
        public Task SendEmailAsync(string toEmail, string subject, string message, bool isMessageHtml = false)
        {
            
                using (var client = new SmtpClient())
                {

                    var credentials = new NetworkCredential()
                    {
                        UserName = "zarrabtest", // without @gmail.com
                        Password = "Mhzsam123"
                    };

                    client.Credentials = credentials;
                    client.Host = "smtp.gmail.com";
                    client.Port = 587;
                    client.EnableSsl = true;

                    using var emailMessage = new MailMessage()
                    {
                        To = { new MailAddress(toEmail) },
                        From = new MailAddress("zarrabtest@gmail.com"), // with @gmail.com
                        Subject = subject,
                        Body = message,
                        IsBodyHtml = isMessageHtml
                    };

                    client.Send(emailMessage);
                }

                return Task.CompletedTask;
            
        }
    }
}