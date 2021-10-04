using EmailSender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSender.Dat
{
    public class TopicsRepository : Repository<Topic>, ITopicsRepository
    {
        public TopicsRepository(ApplicationDbContext context) : base(context){
        }
    }
}
