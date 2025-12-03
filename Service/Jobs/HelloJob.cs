using Core;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NLog;
using Quartz;

namespace Service.Jobs;

[Service(ServiceType.Singleton)]
public class HelloJob : IJob
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    public static void Schedule(IScheduler scheduler)
    {
        var jobDetail = JobBuilder.Create<HelloJob>().WithDescription("HelloJob").Build();

        // var sampleTrigger = TriggerBuilder.Create().StartNow().WithCronSchedule("* * * * * ? *").Build();
        var jobTrigger = TriggerBuilder.Create()
            .StartAt(new DateTimeOffset(DateTime.Parse("2025-01-01 00:00"), new TimeSpan(8, 0, 0)))
            .WithSimpleSchedule(x => x.WithIntervalInSeconds(1).RepeatForever())
            .Build();

        // await scheduler.ScheduleJob(sampleJob, sampleTrigger);
        scheduler.ScheduleJob(jobDetail, jobTrigger).Wait();
    }

    public Task Execute(IJobExecutionContext context)
    {
        Db db = new Db();

        var user = db.User.AsNoTracking()
            .Where(e => e.Id == 1)
            .Take(1)
            .SingleOrDefault();
        if (user != null)
        {
            Console.WriteLine(user.LoginName);
        }

        return Task.CompletedTask;
    }
}