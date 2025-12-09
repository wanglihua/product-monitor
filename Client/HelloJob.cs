using System;
using System.Threading.Tasks;
using Quartz;

namespace Client
{
    public class HelloJob : IJob
    {
        public static void Schedule(IScheduler scheduler)
        {
            var jobDetail = JobBuilder.Create<HelloJob>().WithDescription("HelloJob").Build();

            var jobTrigger = TriggerBuilder.Create()
                .StartAt(new DateTimeOffset(DateTime.Parse("2025-01-01 00:00"), new TimeSpan(8, 0, 0)))
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(5).RepeatForever())
                .Build();

            scheduler.ScheduleJob(jobDetail, jobTrigger).Wait();
        }

        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Hello World from client!");
            return Task.CompletedTask;
        }
    }
}
