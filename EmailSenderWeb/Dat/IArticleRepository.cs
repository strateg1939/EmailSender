using EmailSender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSender.Dat
{
    public interface IArticleRepository : IRepository<Article>
    {
        IEnumerable<Article> GetArticlesWithTopics(string creatorId);
        Article GetArticleWithTopic(int articleId);
        IEnumerable<AspNetUser> GetUsersToSend(Article article);
        bool ArtcileExists(int id);
        void AddConnection(connection_user_article connection);
        bool IsEmailNeededToBeSent(string userId, Article article);
    }
}
