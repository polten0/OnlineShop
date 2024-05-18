using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace OnlineShop_4M_Utility
{
    public class EmailSender : IEmailSender
    {
        private IConfiguration configuration;

        public EmailSender(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var settings = configuration
                .GetSection("EmailSenderSettings")
                .Get<EmailSenderSettings>();

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587);
            await client.AuthenticateAsync(settings.Login, settings.Password);

            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(settings.FromEmail));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;
            message.Body = new BodyBuilder()
            {
                HtmlBody = htmlMessage
            }.ToMessageBody();

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
