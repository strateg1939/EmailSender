using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSender.Dto
{
    public class TopicsPostDto
    {
        public List<int> To_Subscribe { get; set; }
        public List<int> To_Unsubscribe { get; set; }
    }
}
