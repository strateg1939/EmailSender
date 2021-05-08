using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSender.Models
{
    public class Article
    {
        public int ArticleId { get; set; }
        public int TopicID { get; set; }
        [Column(TypeName="date")]
        public DateTime date { get; set; }
        [Column(TypeName ="TEXT")]
        public string Article_text { get; set; }
        public Topic Topic { get; set; }
        public ICollection<connection_user_article> connection_User_Articles { get; set; }

    }
}
