using EmailSender.Models;
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
        TopicsContext _context;
        private const string fromPassword = "I11072003van";
        public readonly MailAddress fromAddress = new MailAddress("newdomain.subscription@gmail.com", "From Name");
        public readonly SmtpClient smtpClient;
        public EmailService(TopicsContext context)
        {
            _context = context;
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
        public void GetAllEmailsToSend(string userId, string userMail, int topicId)
        {
            string topic = _context.Topics.First(i => i.TopicId == topicId).Topic_name;            
            string subject = "Your daily " + topic + " article";
            var possibleArticles = _context.Articles.Where(c => c.TopicID == topicId && c.date.Date == DateTime.Today);
            foreach (var possibleArticle in possibleArticles)
            {
                _context.Entry(possibleArticle).Collection(p => p.connection_User_Articles).Load();
                if (possibleArticle.connection_User_Articles == null || !possibleArticle.connection_User_Articles.Any(c => c.AspNetUserId == userId && c.ArticleId == possibleArticle.ArticleId))
                {
                    SendEmail(possibleArticle, userId, userMail, subject);
                }
            }
            
        }
        public void SendEmail(Article article, string userId, string userMail, string subject)
        {
            var toAddress = new MailAddress(userMail, "To Name");
            string body = article.Article_text;
            var addLink = new connection_user_article { ArticleId = article.ArticleId, AspNetUserId = userId };
            _context.connection_user_article.Add(addLink);
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtpClient.Send(message);
            }
            _context.SaveChanges();
        }
    }
}
