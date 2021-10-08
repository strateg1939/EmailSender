using EmailSender.Dat;
using EmailSender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmailSender.Services
{
    public class ArticleEmailService
    {
        private readonly IUnitOfWork _unitOfWork;
        public readonly IEmailSender _senderService;
        public ArticleEmailService(IUnitOfWork unitOfWork, IEmailSender senderService)
        {
            _unitOfWork = unitOfWork;
            _senderService = senderService;
        }
        public async Task SendNecessaryArticlesToUser(AspNetUser user, int topicId)
        {
            string topic = _unitOfWork.TopicsRepository.Get(topicId).Topic_name;            
            string subject = "Your daily " + topic + " article";
            var possibleArticles = _unitOfWork.ArticleRepository.Find(a => a.TopicID == topicId && a.date.Date == DateTime.Today);
            foreach (var possibleArticle in possibleArticles)
            {
                if (_unitOfWork.ArticleRepository.IsEmailNeededToBeSent(user.Id, possibleArticle)) 
                {
                    await SendArticleEmail(possibleArticle, user, subject);
                }
            }            
        }
        public async Task SendArticleEmail(Article article, AspNetUser user, string subject)
        {           
            string body = article.Article_text;
            var addLink = new connection_user_article { ArticleId = article.ArticleId, AspNetUserId = user.Id };
            _unitOfWork.ArticleRepository.AddConnection(addLink);
            await _senderService.SendMessage(user.Email, subject, body);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
