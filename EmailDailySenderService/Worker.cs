using EmailSender.Models;
using EmailSender.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmailDailySenderService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<ArticleEmailService>();
                var usersAndTopics = dbContext.AspNetUsers
                    .Join(dbContext.connection_user_topic, user => user.Id, connection => connection.AspNetUserID, (user, connection) => 
                    new { Id = user.Id, Email = user.Email, TopicId = connection.TopicID });
                foreach (var item in usersAndTopics)
                {
                    var user = new AspNetUser { Id = item.Id, Email = item.Email };
                    int topic = item.TopicId;
                    await emailService.SendNecessaryArticlesToUser(user, topic);
                }
            }
        }
    }
}
