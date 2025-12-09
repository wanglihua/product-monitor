using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.UI;
using Newtonsoft.Json;
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
            var useDataList = PM.DequeueData(1); // 每次取一个吧

            if (useDataList.Count == 0)
            {
                return Task.CompletedTask;
            }

            string cpuSerialNo = HardwareInfoUtil.GetCpuId();
            string netMacAddress = HardwareInfoUtil.GetMacAddress();

            foreach (var useData in useDataList)
            {
                var param = new ProductInstallDataCollectParam {
                    Key = PM.ClientKey,
                    UseTime = useData.UseTime.ToString(ProductInstallDataCollectParam.DateTimeFormat),
                    AccountType = useData.AccountType,
                    LoginUser = useData.Account,
                    SendTime = DateTime.Now.ToString(ProductInstallDataCollectParam.DateTimeFormat),
                    CpuSerialNo = cpuSerialNo,
                    NetMacAddress = netMacAddress,
                };

                param.Sign = DataCollectUtil.CalSign(param);

                var httpClient = new HttpClient();
                string url = $"{PM.PMSiteBase}/product/install/data/collect";
                var content = new StringContent(JsonConvert.SerializeObject(param));
                httpClient.PostAsync(url, content).GetAwaiter().GetResult();
            }

            return Task.CompletedTask;
        }
    }
}