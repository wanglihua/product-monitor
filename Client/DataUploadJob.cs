using System;
using System.Net.Http;
using System.Threading.Tasks;
using Quartz;

namespace Client
{
    public class DataUploadJob : IJob
    {
        public static void Schedule(IScheduler scheduler)
        {
            var jobDetail = JobBuilder.Create<DataUploadJob>().WithDescription("DataUploadJob").Build();

            var jobTrigger = TriggerBuilder.Create()
                .StartAt(new DateTimeOffset(DateTime.Parse("2025-01-01 00:00"), new TimeSpan(8, 0, 0)))
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever())
                .Build();

            scheduler.ScheduleJob(jobDetail, jobTrigger).Wait();
        }

        public Task Execute(IJobExecutionContext context)
        {
            var useDataList = PM.DequeueData(1);

            if (useDataList.Count == 0)
            {
                return Task.CompletedTask;
            }

            foreach (var useData in useDataList)
            {
                var param = new ProductInstallDataCollectParam {
                };

                var httpClient = new HttpClient();
                var content = new StringContent("");
                var result = httpClient.PostAsync("", content).GetAwaiter().GetResult();
            }

            return Task.CompletedTask;
        }
    }
}