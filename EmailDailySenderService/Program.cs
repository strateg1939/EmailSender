using EmailSender.Models;
using EmailSender.Services;
using EmailSender.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailDailySenderService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {                                      
                    services.Configure<EmailSettings>(hostContext.Configuration.GetSection("MailSettings"));
                    services.AddSingleton<IEmailSender, ConsoleEmailSenderService>();
                    services.AddScoped<ArticleEmailService>();
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlite(hostContext.Configuration.GetConnectionString("DefaultConnection")));
                    services.AddHostedService<Worker>();
                });
    }
}
