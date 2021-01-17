using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSender.Models
{
    public class connection_user_topic
    {
        public string AspNetUserID { get; set; }
        public int TopicID { get; set; }
        public Topic Topic;
        public AspNetUser AspNetUser;
    }
}
