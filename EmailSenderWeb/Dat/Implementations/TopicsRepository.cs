using EmailSender.Models;
using Microsoft.EntityFrameworkCore;
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
        public ApplicationDbContext DbContext { get => _dbContext as ApplicationDbContext; }

        public void AddConnection(connection_user_topic connection)
        {
            DbContext.connection_user_topic.Add(connection);
        }

        public IEnumerable<Topic> GetSubscribedTopics(string userId)
        {
            return DbContext.connection_user_topic
                .Where(connection => connection.AspNetUserID == userId)
                .Include(connection => connection.Topic)
                .Select(connection => connection.Topic);
        }

        public IEnumerable<Topic> GetUnsubscribedTopics(string userId)
        {
            return from topic in DbContext.Topics
            where !(from c in DbContext.connection_user_topic where c.AspNetUserID == userId select c.TopicID).Contains(topic.TopicId)
            select topic;
        }

        public void RemoveConnections(IEnumerable<connection_user_topic> connections)
        {
            DbContext.connection_user_topic.RemoveRange(connections);
        }
    }
}
