using System.Threading.Tasks;

namespace RideShareConnect.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }

    public class MockEmailService : IEmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            Console.WriteLine($"Sending email to {toEmail}: Subject={subject}, Body={body}");
            await Task.CompletedTask;
        }
    }
}