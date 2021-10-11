using EmailSender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSender.Dto
{
    public class TopicsGetDto
    {
        public List<Topic> topicsToAdd { get; set; }
        public List<Topic> topicsToRemove { get; set; }
    }
}
