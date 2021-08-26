using EmailSender.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmailSender.Services
{
    public class EmailSenderService
    {
        private readonly EmailSettings _emailSettings;
        public readonly SmtpClient smtpClient;
        public EmailSenderService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
            smtpClient = new SmtpClient
            {
                Host = _emailSettings.Host,
                Port = _emailSettings.Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailSettings.MailFrom, _emailSettings.Password)
            };
        }
        public async Task SendMessage(string emailAddress, string subject, string body)
        {
            var fromAddress = new MailAddress(_emailSettings.MailFrom, _emailSettings.MailUser);
            var toAddress = new MailAddress(emailAddress);
            using 
            (
                var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                }
            )
            {
                await smtpClient.SendMailAsync(message);
            }
        }
    }
}
