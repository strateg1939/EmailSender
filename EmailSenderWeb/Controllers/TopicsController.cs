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
using EmailSender.Dat;

namespace EmailSender.Controllers
{
    public class TopicsController : Controller
    {
        private readonly ArticleEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AspNetUser> _userManager;


        public TopicsController(IUnitOfWork unitOfWork, ArticleEmailService emailService, UserManager<AspNetUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            Dictionary<int, string> topicsThatCanUnsubscribed = new();
            Dictionary<int, string> topicsThatCanSubscribed = new();
            FindTopicsForUser(currentUserID, topicsThatCanUnsubscribed, topicsThatCanSubscribed);

            ViewData["topicsToRemove"] = topicsThatCanUnsubscribed;
            ViewData["topicsToAdd"] = topicsThatCanSubscribed;
            return View();
        }

        private void FindTopicsForUser(string currentUserID, Dictionary<int, string> topicsThatCanUnsubscribed, Dictionary<int, string> topicsThatCanSubscribed)
        {
            var subscribedTopics = _unitOfWork.TopicsRepository.GetSubscribedTopics(currentUserID);
            var notSubscribedTopics = _unitOfWork.TopicsRepository.GetUnsubscribedTopics(currentUserID);

            foreach (var subscribedTopic in subscribedTopics)
            {
                topicsThatCanUnsubscribed.Add(subscribedTopic.TopicId, subscribedTopic.Topic_name);
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
                connection_user_topic newLine = new();
                newLine.TopicID = topicId;
                newLine.AspNetUserID = user.Id;
                await _emailService.SendNecessaryArticlesToUser(user, topicId);
                _unitOfWork.TopicsRepository.AddConnection(newLine);
            }
            var toUnsubscribe = topicsPostDto.To_Unsubscribe.Select(id => new connection_user_topic { AspNetUserID = user.Id, TopicID = id });
            _unitOfWork.TopicsRepository.RemoveConnections(toUnsubscribe);
            await _unitOfWork.SaveChangesAsync();
            return "success";
        }
    }
}
