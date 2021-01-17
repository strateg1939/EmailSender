using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSender.Models
{
    public class connection_user_article
    {
        public string AspNetUserId { get; set; }
        public AspNetUser AspNetUser { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
