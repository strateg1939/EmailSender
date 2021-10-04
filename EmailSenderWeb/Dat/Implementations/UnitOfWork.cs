using EmailSender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSender.Dat
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            ArticleRepository = new ArticleRepository(dbContext);
            TopicsRepository = new TopicsRepository(dbContext);
        }

        public IArticleRepository ArticleRepository { get; private set; }
        public ITopicsRepository TopicsRepository { get; private set; }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
