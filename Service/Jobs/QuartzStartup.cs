using System.Collections.Specialized;
using NLog;
using Quartz;
using Quartz.Impl;

namespace Service.Jobs;

public class QuartzStartup(IServiceProvider serviceProvider)
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private IScheduler? scheduler;

    // public async Task Start()
    public void Start()
    {
        if (scheduler is not null)
        {
            throw new InvalidOperationException("Already started.");
        }

        var properties = new NameValueCollection {
            ["quartz.serializer.type"] = "binary",
        };

        var schedulerFactory = new StdSchedulerFactory(properties);

        // scheduler = await schedulerFactory.GetScheduler();
        scheduler = schedulerFactory.GetScheduler().Result;
        scheduler.JobFactory = new JobFactory(serviceProvider);
        // await scheduler.Start();
        scheduler.Start().Wait();

// #if DEBUG
//         HelloJob.Schedule(scheduler);
// #endif

#if !DEBUG
        ProductSaleRentEvalTimesCalJob.Schedule(scheduler);
#endif
    }

    public void Stop()
    {
        if (scheduler is null)
        {
            return;
        }

        // give running jobs 10 sec to stop gracefully
        if (scheduler.Shutdown(waitForJobsToComplete: true).Wait(10000))
        {
            scheduler = null;
        }
        else
        {
            log.Error("系统关闭，但有运行中的定时任务未能完成！");
        }
    }
}