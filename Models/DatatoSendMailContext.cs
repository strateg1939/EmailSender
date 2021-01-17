using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EmailSender.Models
{
    public partial class DatatoSendMailContext : DbContext
    {
        public DatatoSendMailContext()
        {
        }

        public DatatoSendMailContext(DbContextOptions<DatatoSendMailContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlite("Data Source=DatatoSendMail.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
