using Business_Logic.Services.IServices;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Business_Logic.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            using var client = new SmtpClient
            {
                Host = "smtp.yourmailserver.com",
                Port = 587,
                Credentials = new NetworkCredential("your-email@example.com", "your-email-password"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage("your-email@example.com", email)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            await client.SendMailAsync(mailMessage);
        }
    }
}