using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmailSender.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

using System.Net;
using System.Net.Mail;
using EmailSender.Services;

namespace EmailSender.Controllers
{
    public class TopicsController : Controller
    {
        private EmailService _emailService;
        private TopicsContext _context;
        private string currentUserID;
        private string currentUserMail;
       

        public TopicsController(TopicsContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
       
           
    

        [Authorize]
        public IActionResult Index()
        {
            ClaimsPrincipal currentUser = this.User;
            currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var topicsToRemove = from p in _context.Topics join c in _context.connection_user_topic on p.TopicId equals c.TopicID where c.AspNetUserID == currentUserID select new { p.Topic_name, c.TopicID };
            var topicsToAdd = from p in _context.Topics where !(from c in _context.connection_user_topic where c.AspNetUserID == currentUserID select c.TopicID).Contains(p.TopicId) select new { p.TopicId, p.Topic_name };
            Dictionary<int, string> removeTopics = new Dictionary<int, string>();
            Dictionary<int, string> addTopics = new Dictionary<int, string>();
            foreach (var topicToRemove in topicsToRemove)
            {
                removeTopics.Add(topicToRemove.TopicID, topicToRemove.Topic_name);
            }
            ViewData["topicsToRemove"] = removeTopics; 

            foreach(var topicToAdd in topicsToAdd)
            {
                addTopics.Add(topicToAdd.TopicId, topicToAdd.Topic_name);
            }
            ViewData["topicsToAdd"] = addTopics;
            return View();
        }

        [HttpPost]
        [Route("api/[controller]")]
        public string PostController([FromBody] Dictionary<string, List<int>> sendMail)
        {
            ClaimsPrincipal currentUser = this.User;
            currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            currentUserMail = currentUser.FindFirst(ClaimTypes.Email).Value;
            List<string> sendMailList = new List<string>(sendMail.Keys);
            foreach(var item in sendMail[sendMailList[0]])
            {
                connection_user_topic newLine = new connection_user_topic();
                newLine.TopicID = item;
                newLine.AspNetUserID = currentUserID;
                sendEmail(currentUserID, currentUserMail, item);
                _context.connection_user_topic.Add(newLine);
            }
            if (sendMail[sendMailList[1]].Any())
            {
                foreach (var item in sendMail[sendMailList[1]])
                {
                    var deleteTopic = _context.connection_user_topic.Where(c => c.AspNetUserID == currentUserID && c.TopicID == item).First();
                    _context.connection_user_topic.Remove(deleteTopic);
                }
            }
             _context.SaveChanges();
            return "success";
            
        }

        public void sendEmail(string userId, string userMail, int topicId)
        {

            string topic = _context.Topics.First(i => i.TopicId == topicId).Topic_name;

            var toAddress = new MailAddress(userMail, "To Name");

            string subject = "Your daily " + topic + " article";

            var possibleArticles = _context.Articles.Where(c => c.TopicID == topicId && c.date.Date == DateTime.Today);

            foreach (var possibleArticle in possibleArticles)
            {
                _context.Entry(possibleArticle).Collection(p => p.connection_User_Articles).Load();
                if (possibleArticle.connection_User_Articles == null || !possibleArticle.connection_User_Articles.Any(c => c.AspNetUserId == userId && c.ArticleId == possibleArticle.ArticleId))
                {
                    string body = possibleArticle.Article_text;
                    var addLink = new connection_user_article { ArticleId = possibleArticle.ArticleId, AspNetUserId = userId };
                    _context.Add(addLink);
                    using (var message = new MailMessage(_emailService.fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        _emailService.smtpClient.Send(message);
                    }

                }
            }
            _context.SaveChanges();




        }
    }
}
