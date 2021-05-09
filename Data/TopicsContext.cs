using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSender.Models
{
    public class TopicsContext : DbContext
    {
        public TopicsContext(DbContextOptions
    <TopicsContext> options) : base(options)
        {
        }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<connection_user_topic> connection_user_topic { get; set; }
        public DbSet<Article> Articles { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<connection_user_article> connection_user_article { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<connection_user_article>().HasKey(i => new { i.ArticleId, i.AspNetUserId });
            modelBuilder.Entity<connection_user_topic>().HasKey(i => new { i.TopicID, i.AspNetUserID });
            modelBuilder.Entity<connection_user_article>()
            .HasOne(p => p.Article)
            .WithMany(b => b.connection_User_Articles)
            .HasForeignKey(p => p.ArticleId);


            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("nvarchar(450)");

                entity.Property(e => e.AccessFailedCount).HasColumnType("int");

                entity.Property(e => e.Email).HasColumnType("nvarchar(256)");

                entity.Property(e => e.EmailConfirmed)
                    .IsRequired()
                    .HasColumnType("bit");

                entity.Property(e => e.LockoutEnabled)
                    .IsRequired()
                    .HasColumnType("bit");

                entity.Property(e => e.LockoutEnd).HasColumnType("datetimeoffset");

                entity.Property(e => e.NormalizedEmail).HasColumnType("nvarchar(256)");

                entity.Property(e => e.NormalizedUserName).HasColumnType("nvarchar(256)");

                entity.Property(e => e.PhoneNumberConfirmed)
                    .IsRequired()
                    .HasColumnType("bit");

                entity.Property(e => e.TwoFactorEnabled)
                    .IsRequired()
                    .HasColumnType("bit");

                entity.Property(e => e.UserName).HasColumnType("nvarchar(256)");
            });
        }

    
    

    }
}
