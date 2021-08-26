using EmailSender.Controllers;
using EmailSender.Models;
using EmailSender.Services;
using Microsoft.EntityFrameworkCore;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmailSender.Areas
{
    public class EmailSchedulerSender : IJob
    {
        private readonly EmailService _emailService;
        private ApplicationDbContext _context;
        public EmailSchedulerSender(ApplicationDbContext context, EmailService emailService)
        {
            _emailService = emailService;
            _context = context;
        }

        public async Task Execute(IJobExecutionContext executionContext)
        {
            var usersAndTopics = _context.AspNetUsers.Join(
                _context.connection_user_topic, user => user.Id, connection => connection.AspNetUserID, (user, connection) => new { Id = user.Id, Email = user.Email, TopicId = connection.TopicID }
            );
            foreach (var item in usersAndTopics)
            {
                var user = new AspNetUser { Id = item.Id, Email = item.Email };
                int topic = item.TopicId;
                await _emailService.SendNecessaryArticlesToUser(user, topic);
            }
        }
    }
}
