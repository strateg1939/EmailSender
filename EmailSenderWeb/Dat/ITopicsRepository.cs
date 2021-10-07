using EmailSender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSender.Dat
{
    public interface ITopicsRepository : IRepository<Topic>
    {
        public IEnumerable<Topic> GetSubscribedTopics(string userId);
        public IEnumerable<Topic> GetUnsubscribedTopics(string userId);
        public void AddConnection(connection_user_topic connection);
        public void RemoveConnections(IEnumerable<connection_user_topic> connections);
    }
}
