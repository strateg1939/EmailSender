using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSender.Areas
{
    public class EmailScheduler
    {
        public static async void Start()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<Sender>().Build();

            ITrigger trigger = TriggerBuilder.Create()  
                .WithIdentity("trigger1", "group1")     
                .StartNow()                            
                .WithSimpleSchedule(x => x            
                    .WithIntervalInHours(24)         
                    .RepeatForever())                   
                .Build();                               

            await scheduler.ScheduleJob(job, trigger);       
        }
    }
}
