using Core;
using Domain;
using NLog;
using Quartz;
using Service.ProductRent;
using Service.ProductSale;

namespace Service.Jobs;

[Service(ServiceType.Singleton)]
public class ProductSaleRentEvalTimesCalJob : IJob
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly int TaskIntervalMinutes = 5;

    public static void Schedule(IScheduler scheduler)
    {
        var jobDetail = JobBuilder.Create<ProductSaleRentEvalTimesCalJob>().WithDescription("ProductSaleRentEvalTimesCalJob").Build();

        var jobTrigger = TriggerBuilder.Create()
            .StartAt(new DateTimeOffset(DateTime.Parse("2025-01-01 00:00"), new TimeSpan(8, 0, 0)))
            .WithSimpleSchedule(x => x.WithIntervalInMinutes(5).RepeatForever())
            .Build();

        scheduler.ScheduleJob(jobDetail, jobTrigger).Wait();
    }

    public Task Execute(IJobExecutionContext context)
    {
        DateTime someTimeAgo = DateTime.Now.Subtract(TimeSpan.FromMinutes(TaskIntervalMinutes * 3));

        Db db = new Db();

        // 销售目录
        var productSaleService = new ProductSaleService(db);

        var productSaleListToUpdate = db.ProductSale.Where(e => e.UpdateTime > someTimeAgo).ToList();
        foreach (var productSale in productSaleListToUpdate)
        {
            long productSaleId = productSale.Id;
            productSaleService.UpdateProductSaleByLatestData(productSaleId);
        }

        // 租赁目录
        var productRentService = new ProductRentService(db);
        var productRentListToUpdate = db.ProductRent.Where(e => e.UpdateTime > DateTime.Now).ToList();
        foreach (var productRent in productRentListToUpdate)
        {
            long productRentId = productRent.Id;
            productRentService.UpdateProductRentByLatestData(productRentId);
        }

        return Task.CompletedTask;
    }
}