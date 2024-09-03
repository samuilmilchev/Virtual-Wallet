using System.Net;
using System.Net.Mail;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Services.Contracts;

namespace Virtual_Wallet.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IConfiguration configuration)
        {
            _smtpSettings = configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
        }



        public async Task SendAsync(string email, string subject, string body)
        {
            using (var client = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port))
            {
                client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                client.EnableSsl = _smtpSettings.EnableSsl;

                var mailMessage = new MailMessage()
                {
                    From = new MailAddress(_smtpSettings.SenderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true

                };

                mailMessage.To.Add(email);

                try
                {
                    await client.SendMailAsync(mailMessage);
                }
                catch (Exception x)
                {

                    throw new InvalidOperationException($"Error sending e-mail. {x.Message}");
                }
            }
        }
    }
}
