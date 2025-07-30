// using System.Threading.Tasks;

// namespace RideShareConnect.Services
// {
//     public interface IEmailService
//     {
//         Task SendEmailAsync(string toEmail, string subject, string body);
//     }

//     public class MockEmailService : IEmailService
//     {
//         public async Task SendEmailAsync(string toEmail, string subject, string body)
//         {
//             Console.WriteLine($"Sending email to {toEmail}: Subject={subject}, Body={body}");
//             await Task.CompletedTask;
//         }
//     }
// }


using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace RideShareConnect.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["EmailSettings:SenderEmail"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config["EmailSettings:SmtpHost"], int.Parse(_config["EmailSettings:Port"]), SecureSocketOptions.SslOnConnect);
            await smtp.AuthenticateAsync(_config["EmailSettings:SenderEmail"], _config["EmailSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}