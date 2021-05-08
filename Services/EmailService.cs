using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmailSender.Services
{
    public class EmailService
    {
        private const string fromPassword = "I11072003van";
        public readonly MailAddress fromAddress = new MailAddress("newdomain.subscription@gmail.com", "From Name");
        public readonly SmtpClient smtpClient;
        public EmailService()
        {
            smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
        }
    }
}
