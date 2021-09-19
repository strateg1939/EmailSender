using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSender.Services
{
    public interface IEmailSender
    {
        Task SendMessage(string emailAddress, string subject, string body);
    }
}
