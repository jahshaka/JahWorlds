using System.Threading.Tasks;

namespace Jahshaka.Web.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
