using System.Threading.Tasks;

namespace UdemyEgitimPlatformu.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}