using EmailSender.Controllers;
using EmailSender.Dat;
using EmailSender.Dto;
using EmailSender.Models;
using EmailSender.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSenderTests
{
    public class TopicsControllerTests
    {
        private Mock<ArticleEmailService> mockEmailService;
        private Mock<ITopicsRepository> topicsRepository;
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<UserManager<AspNetUser>> mockManager;
        private AspNetUser user = new AspNetUser { Id = "1" };

        [SetUp]
        public void Setup()
        {
            unitOfWork = new();
            var mockSenderService = new Mock<IEmailSender>();
            mockEmailService = new Mock<ArticleEmailService>(unitOfWork.Object, mockSenderService.Object);
            mockEmailService.Setup(x => x.SendNecessaryArticlesToUser(It.IsAny<AspNetUser>(), It.IsAny<int>()));
            var store = new Mock<IUserStore<AspNetUser>>();
            mockManager = new Mock<UserManager<AspNetUser>>(store.Object, null, null, null, null, null, null, null, null);
            mockManager.Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            topicsRepository = new();
            unitOfWork.Setup(u => u.TopicsRepository).Returns(topicsRepository.Object);
        }

        [Test]
        public void Index_ReturnsModelOfTypeTopicsGetDto()
        {
            var topicsToAdd = new List<Topic>();
            var topicsToRemove = new List<Topic>();
            topicsRepository.Setup(t => t.GetSubscribedTopics(It.IsAny<string>())).Returns(topicsToAdd);
            topicsRepository.Setup(t => t.GetUnsubscribedTopics(It.IsAny<string>())).Returns(topicsToRemove);
            var topicsController = new TopicsController(unitOfWork.Object, mockEmailService.Object, mockManager.Object);

            var result = topicsController.Index();

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOf<TopicsGetDto>(viewResult.Model);
        }
        [Test]
        public void Index_ModelReturnsTopicsToAddAndRemove()
        {
            var unsubscribedTopics = new List<Topic>
            {
                new Topic
                {
                    TopicId = 1
                }
            };
            var subscribedTopics = new List<Topic>
            {
                new Topic
                {
                    TopicId = 2
                }
            };
            topicsRepository.Setup(t => t.GetSubscribedTopics(It.IsAny<string>())).Returns(subscribedTopics);
            topicsRepository.Setup(t => t.GetUnsubscribedTopics(It.IsAny<string>())).Returns(unsubscribedTopics);
            var topicsController = new TopicsController(unitOfWork.Object, mockEmailService.Object, mockManager.Object);

            var result = topicsController.Index();

            var viewResult = (ViewResult)result;
            var model = (TopicsGetDto)viewResult.Model;
            Assert.AreEqual(unsubscribedTopics, model.topicsToAdd);
            Assert.AreEqual(subscribedTopics, model.topicsToRemove);
        }
        public TopicsPostDto SetupForPost(List<connection_user_topic> addedConnections)
        {
            var topicsPostDto = new TopicsPostDto
            {
                To_Subscribe = new(),
                To_Unsubscribe = new()
            };
            topicsRepository.Setup(t => t.AddConnection(It.IsAny<connection_user_topic>())).Callback((connection_user_topic connection) => addedConnections.Add(connection));
            topicsRepository.Setup(t => t.RemoveConnections(It.IsAny<IEnumerable<connection_user_topic>>()));
            return topicsPostDto;
        }
        [Test]
        public async Task Post_ReturnsSuccessAsync()
        {
            var addedConnections = new List<connection_user_topic>();
            var removedConnections = new List<connection_user_topic>();
            var postDto = SetupForPost(addedConnections);
            var topicsController = new TopicsController(unitOfWork.Object, mockEmailService.Object, mockManager.Object);

            var result = await topicsController.PostControllerAsync(postDto);

            Assert.AreEqual("success", result);
        }
        [Test]
        public async Task Post_AddsConnectionsAndCallsEmailServiceAsync()
        {
            var addedConnections = new List<connection_user_topic>();
            var removedConnections = new List<connection_user_topic>();
            var postDto = SetupForPost(addedConnections);
            var toSubscribe = new List<int> { 1, 2 };
            postDto.To_Subscribe = toSubscribe;
            var topicsController = new TopicsController(unitOfWork.Object, mockEmailService.Object, mockManager.Object);
            
            await topicsController.PostControllerAsync(postDto);

            Assert.AreEqual(toSubscribe.Count, addedConnections.Count);
            Assert.AreEqual(toSubscribe[0], addedConnections[0].TopicID);
            Assert.AreEqual(toSubscribe[1], addedConnections[1].TopicID);
            Assert.AreEqual(user.Id, addedConnections[0].AspNetUserID);
            mockEmailService.Verify(x => x.SendNecessaryArticlesToUser(It.IsAny<AspNetUser>(), It.IsAny<int>()), Times.Exactly(toSubscribe.Count));
        }
        [Test]
        public async Task Post_CallsRemoveConnections()
        {
            var addedConnections = new List<connection_user_topic>();
            var connectionsToRemove = new List<connection_user_topic>
            {
                new connection_user_topic
                {
                    AspNetUserID = user.Id,
                    TopicID = 1
                }
            };
            var postDto = SetupForPost(addedConnections);
            var toUnsubscribe = new List<int> { 1 };
            postDto.To_Unsubscribe = toUnsubscribe;
            var topicsController = new TopicsController(unitOfWork.Object, mockEmailService.Object, mockManager.Object);

            await topicsController.PostControllerAsync(postDto);

            topicsRepository.Verify(x => x.RemoveConnections(It.Is<IEnumerable<connection_user_topic>>(
                list => list.Count() == 1 &&
                list.First().AspNetUserID == connectionsToRemove[0].AspNetUserID &&
                list.First().TopicID == connectionsToRemove[0].TopicID)));           
        }
    }
}