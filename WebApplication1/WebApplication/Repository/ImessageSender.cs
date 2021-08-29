using System.Threading.Tasks;

namespace WebApplication.Repository
{
    public interface ImessageSender
    {
        public Task SendEmailAsync(string emailtoEmail, string subject, string message, bool isMessageHtml = false);
    }
}