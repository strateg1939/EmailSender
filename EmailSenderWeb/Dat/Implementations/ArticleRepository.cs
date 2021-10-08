using EmailSender.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSender.Dat
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        public ArticleRepository(ApplicationDbContext context) : base(context){
        }
        public IEnumerable<Article> GetArticlesWithTopics(string creatorId)
        {
            return DbContext.Articles
                .Where(article => article.CreatorID == creatorId)
                .Include(article => article.Topic)
                .ToList();
        }

        public Article GetArticleWithTopic(int articleId)
        {
           return DbContext.Articles
                .Include(m => m.Topic)
                .FirstOrDefault(m => m.ArticleId == articleId);
        }
        public IEnumerable<AspNetUser> GetUsersToSend(Article article)
        {
            return DbContext.connection_user_topic.Where(con => con.TopicID == article.TopicID).Include(con => con.AspNetUser).Select(con => con.AspNetUser);
        }
        public bool ArtcileExists(int id)
        {
            return DbContext.Articles.Any(e => e.ArticleId == id);
        }

        public void AddConnection(connection_user_article connection)
        {
            DbContext.connection_user_article.Add(connection);
        }

        public bool IsEmailNeededToBeSent(string userId, Article article)
        {
            DbContext.Entry(article).Collection(p => p.connection_User_Articles).Load();
            return article.connection_User_Articles == null || !article.connection_User_Articles.Any(c => c.AspNetUserId == userId && c.ArticleId == article.ArticleId);
        }

        public ApplicationDbContext DbContext { get => _dbContext as ApplicationDbContext; }
    }
}
