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
using EmailSender.Dto;

namespace EmailSender.Controllers
{
    public class TopicsController : Controller
    {
        private readonly ArticleEmailService _emailService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AspNetUser> _userManager;


        public TopicsController(ApplicationDbContext context, ArticleEmailService emailService, UserManager<AspNetUser> userManager)
        {
            _context = context;
            _emailService = emailService;
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            Dictionary<int, string> topicsThatCanUnsubscribed = new Dictionary<int, string>();
            Dictionary<int, string> topicsThatCanSubscribed = new Dictionary<int, string>();
            FindTopicsForUser(currentUserID, topicsThatCanUnsubscribed, topicsThatCanSubscribed);

            ViewData["topicsToRemove"] = topicsThatCanUnsubscribed;
            ViewData["topicsToAdd"] = topicsThatCanSubscribed;
            return View();
        }

        private void FindTopicsForUser(string currentUserID, Dictionary<int, string> topicsThatCanUnsubscribed, Dictionary<int, string> topicsThatCanSubscribed)
        {
            var subscribedTopics = _context.connection_user_topic
                .Where(connection => connection.AspNetUserID == currentUserID)
                .Include(connection => connection.Topic)
                .Select(connection => new { connection.Topic.Topic_name, connection.TopicID });
            var notSubscribedTopics = from topic in _context.Topics
                                      where
!(from c in _context.connection_user_topic where c.AspNetUserID == currentUserID select c.TopicID).Contains(topic.TopicId)
                                      select new { topic.TopicId, topic.Topic_name };

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
        public async Task<string> PostControllerAsync([FromBody] TopicsPostDto topicsPostDto)
        {
            ClaimsPrincipal currentUser = this.User;
            var user = await _userManager.GetUserAsync(currentUser);
            foreach (var topicId in topicsPostDto.To_Subscribe)
            {
                connection_user_topic newLine = new connection_user_topic();
                newLine.TopicID = topicId;
                newLine.AspNetUserID = user.Id;
                await _emailService.SendNecessaryArticlesToUser(user, topicId);
                _context.connection_user_topic.Add(newLine);
            }
            foreach (var topicId in topicsPostDto.To_Unsubscribe)
            {
                var deleteTopic = _context.connection_user_topic.Where(c => c.AspNetUserID == user.Id && c.TopicID == topicId).First();
                _context.connection_user_topic.Remove(deleteTopic);
            }
            _context.SaveChanges();
            return "success";
        }
    }
}
