﻿// <auto-generated />
using System;
using EmailSender.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EmailSender.Migrations
{
    [DbContext(typeof(TopicsContext))]
    [Migration("20210114200728_migr")]
    partial class migr
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("EmailSender.Models.Article", b =>
                {
                    b.Property<int>("ArticleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Article_text")
                        .HasColumnType("TEXT");

                    b.Property<int>("TopicID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("date")
                        .HasColumnType("date");

                    b.HasKey("ArticleId");

                    b.HasIndex("TopicID");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("EmailSender.Models.AspNetUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<byte[]>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "NormalizedEmail" }, "EmailIndex");

                    b.HasIndex(new[] { "NormalizedUserName" }, "UserNameIndex")
                        .IsUnique();

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("EmailSender.Models.Topic", b =>
                {
                    b.Property<int>("TopicId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Topic_name")
                        .HasColumnType("TEXT");

                    b.HasKey("TopicId");

                    b.ToTable("Topics");

                    b.HasData(
                        new
                        {
                            TopicId = 1,
                            Topic_name = "news"
                        },
                        new
                        {
                            TopicId = 2,
                            Topic_name = "finance"
                        },
                        new
                        {
                            TopicId = 3,
                            Topic_name = "sport"
                        });
                });

            modelBuilder.Entity("EmailSender.Models.connection_user_topic", b =>
                {
                    b.Property<int>("connection_user_topicID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AspNetUserID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("TopicID")
                        .HasColumnType("INTEGER");

                    b.HasKey("connection_user_topicID");

                    b.HasIndex("AspNetUserID");

                    b.HasIndex("TopicID");

                    b.ToTable("connection_user_topic");
                });

            modelBuilder.Entity("EmailSender.Models.Article", b =>
                {
                    b.HasOne("EmailSender.Models.Topic", null)
                        .WithMany("Articles")
                        .HasForeignKey("TopicID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EmailSender.Models.connection_user_topic", b =>
                {
                    b.HasOne("EmailSender.Models.AspNetUser", null)
                        .WithMany("connection_User_Topics")
                        .HasForeignKey("AspNetUserID");

                    b.HasOne("EmailSender.Models.Topic", null)
                        .WithMany("connection_User_Topics")
                        .HasForeignKey("TopicID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EmailSender.Models.AspNetUser", b =>
                {
                    b.Navigation("connection_User_Topics");
                });

            modelBuilder.Entity("EmailSender.Models.Topic", b =>
                {
                    b.Navigation("Articles");

                    b.Navigation("connection_User_Topics");
                });
#pragma warning restore 612, 618
        }
    }
}
