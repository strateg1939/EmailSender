using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSender.Models
{
    public class Topic
    {
        public int TopicId { get; set; }
        public string Topic_name { get; set; }
        public ICollection<connection_user_topic> connection_User_Topics { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
