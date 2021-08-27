using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSender.Models
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<AspNetUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        { }

        public DbSet<Topic> Topics { get; set; }
        public DbSet<connection_user_topic> connection_user_topic { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<connection_user_article> connection_user_article { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>()
                .HasOne(a => a.AspNetUser)
                .WithMany(user => user.Articles)
                .HasForeignKey(a => a.CreatorID)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<connection_user_article>().HasKey(i => new { i.ArticleId, i.AspNetUserId });
            modelBuilder.Entity<connection_user_topic>().HasKey(i => new { i.TopicID, i.AspNetUserID });
            modelBuilder.Entity<connection_user_article>()
                .HasOne(p => p.Article)
                .WithMany(b => b.connection_User_Articles)
                .HasForeignKey(p => p.ArticleId);
            base.OnModelCreating(modelBuilder);
        }

    
    

    }
}
