using Hackathon_2024_INFISOFTWARE.Domain.Configs;
using Hackathon_2024_INFISOFTWARE.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Hackathon_2024_INFISOFTWARE.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly EmailOption _emailConfig;

        public EmailService(IOptions<EmailOption> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var smtpClient = new SmtpClient
                {
                    Host = _emailConfig.Host,
                    Port = _emailConfig.Port,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(_emailConfig.UserName, _emailConfig.Password)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailConfig.UserName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error sending email: {ex.Message}");
            }
        }
    }
}
