using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSender.Services
{
    public class ConsoleEmailSenderService : IEmailSender
    {
        private readonly ILogger<ConsoleEmailSenderService> _logger;
        public ConsoleEmailSenderService(ILogger<ConsoleEmailSenderService> logger)
        {
            _logger = logger;
        }
        public async Task SendMessage(string emailAddress, string subject, string body)
        {
            _logger.LogInformation($"Sending message to {emailAddress} with subject : {subject} \n and body : {body}");
        }
    }
}
