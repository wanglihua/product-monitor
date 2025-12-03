using NLog;
using Quartz;
using Quartz.Spi;

namespace Service.Jobs;

public class JobFactory(IServiceProvider serviceProvider) : IJobFactory
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        string? jobDescription = bundle.JobDetail.Description;
        if (jobDescription == null)
        {
            jobDescription = string.Empty;
        }

        string jobKey = bundle.JobDetail.Key.Name;
        if (string.IsNullOrWhiteSpace(jobKey))
        {
            jobKey = string.Empty;
        }

        if (serviceProvider.GetService(bundle.JobDetail.JobType) is not IJob job)
        {
            string errMessage = $"job ({jobDescription}) 创建失败！Job Key: {jobKey}";
            log.Error(errMessage);

            throw new Exception(errMessage);
        }

#if DEBUG
        Type jobType = job.GetType();
        log.Info($"job {jobType.Name} 创建成功！");
#endif

        return job;
    }

    public void ReturnJob(IJob job)
    {
    }
}