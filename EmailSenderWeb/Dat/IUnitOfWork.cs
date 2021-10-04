using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSender.Dat
{
    public interface IUnitOfWork
    {
        IArticleRepository ArticleRepository { get;}
        ITopicsRepository TopicsRepository { get; }
        Task SaveChangesAsync(); 
    }
}
