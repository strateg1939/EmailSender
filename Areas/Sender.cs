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
    public class Sender : IJob
    {
        private readonly EmailService _emailService;
        private TopicsContext _context;
        public Sender(TopicsContext context, EmailService emailService)
        {
            _emailService = emailService;
            _context = context;
        }

        public async Task Execute(IJobExecutionContext executionContext)
        {
            var usersAndTopics = _context.AspNetUsers.Join(_context.connection_user_topic, u => u.Id, c => c.AspNetUserID, (u, c) => new { Id = u.Id, Email = u.Email, TopicId = c.TopicID }
            );
            foreach (var item in usersAndTopics)
            {
                string userId = item.Id;
                string userMail = item.Email;
                int topic = item.TopicId;
                _emailService.sendEmail(userId, userMail, topic);

            }
        }
    }
}
