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
        private readonly ApplicationDbContext _dbContext;
        public readonly EmailSenderService _senderService;
        public EmailService(ApplicationDbContext context, EmailSenderService senderService)
        {
            _dbContext = context;
            _senderService = senderService;
        }
        public async Task SendNecessaryArticlesToUser(AspNetUser user, int topicId)
        {
            string topic = _dbContext.Topics.First(i => i.TopicId == topicId).Topic_name;            
            string subject = "Your daily " + topic + " article";
            var possibleArticles = _dbContext.Articles.Where(c => c.TopicID == topicId && c.date.Date == DateTime.Today);
            foreach (var possibleArticle in possibleArticles)
            {
                _dbContext.Entry(possibleArticle).Collection(p => p.connection_User_Articles).Load();
                if (possibleArticle.connection_User_Articles == null || !possibleArticle.connection_User_Articles.Any(c => c.AspNetUserId == user.Id && c.ArticleId == possibleArticle.ArticleId))
                {
                    await SendArticleEmail(possibleArticle, user.Id, user.Email, subject);
                }
            }            
        }
        public async Task SendArticleEmail(Article article, string userId, string userMail, string subject)
        {           
            string body = article.Article_text;
            var addLink = new connection_user_article { ArticleId = article.ArticleId, AspNetUserId = userId };
            _dbContext.connection_user_article.Add(addLink);
            await _dbContext.SaveChangesAsync();
            await _senderService.SendMessage(userMail, subject, body);
        }
    }
}
