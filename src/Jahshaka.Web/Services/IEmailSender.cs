using System.Threading.Tasks;

namespace Jahshaka.Web.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
