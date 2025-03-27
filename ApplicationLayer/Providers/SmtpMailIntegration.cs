using ApplicationLayer.IProviders;
using ApplicationLayer.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace ApplicationLayer.Providers
{
    public class SmtpMailIntegration : IEmailProvider
    {
        private readonly SmtpOptions _options;

        public SmtpMailIntegration(IOptions<SmtpOptions> options)
        {
            _options = options.Value;
        }

        public async Task<bool> SendEmailAsync(SendNotificationDto request)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    client.Host = _options.Host;
                    client.Port = _options.Port;
                    client.EnableSsl = _options.EnableSsl;
                    client.Credentials = new NetworkCredential(_options.Username, _options.Password);

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_options.FromAddress),
                        Subject = "[SMTP] Notification Service",
                        Body = request.Message,
                        IsBodyHtml = false
                    };

                    mailMessage.To.Add(new MailAddress(request.Recipient));
                    await client.SendMailAsync(mailMessage);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}