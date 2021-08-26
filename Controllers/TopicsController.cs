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
        private ApplicationDbContext _context;
        private string currentUserID;
        private string currentUserMail;
       

        public TopicsController(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [Authorize]
        public IActionResult Index()
        {
            ClaimsPrincipal currentUser = this.User;
            currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            Dictionary<int, string> topicsThatCanUnsubscribed = new Dictionary<int, string>();
            Dictionary<int, string> topicsThatCanSubscribed = new Dictionary<int, string>();
            FindTopicsForUser(currentUserID, topicsThatCanUnsubscribed, topicsThatCanSubscribed);
           
            ViewData["topicsToRemove"] = topicsThatCanUnsubscribed; 
            ViewData["topicsToAdd"] = topicsThatCanSubscribed;
            return View();
        }

        private void FindTopicsForUser(string currentUserID, Dictionary<int, string> topicsThatCanUnsubscribed, Dictionary<int, string> topicsThatCanSubscribed)
        {
            var subscribedTopics = from p in _context.Topics join c in _context.connection_user_topic on p.TopicId equals c.TopicID where c.AspNetUserID == currentUserID select new { p.Topic_name, c.TopicID };
            var notSubscribedTopics = from p in _context.Topics where !(from c in _context.connection_user_topic where c.AspNetUserID == currentUserID select c.TopicID).Contains(p.TopicId) select new { p.TopicId, p.Topic_name };

            foreach (var subscribedTopic in subscribedTopics)
            {
                topicsThatCanUnsubscribed.Add(subscribedTopic.TopicID, subscribedTopic.Topic_name);
            }
            foreach (var notSubscribedTopic in notSubscribedTopics)
            {
                topicsThatCanSubscribed.Add(notSubscribedTopic.TopicId, notSubscribedTopic.Topic_name);
            }
        }

        [HttpPost]
        [Route("api/[controller]")]
        public async Task<string> PostControllerAsync([FromBody] Dictionary<string, List<int>> sendMail)
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
                await _emailService.SendNecessaryArticlesToUser(currentUserID, currentUserMail, item);
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
    }
}
