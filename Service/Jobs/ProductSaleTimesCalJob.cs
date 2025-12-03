using Core;
using Domain;
using NLog;
using Quartz;
using Service.ProductSale;

namespace Service.Jobs;

[Service(ServiceType.Singleton)]
public class ProductSaleTimesCalJob : IJob
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly int TaskIntervalMinutes = 5;

    public static void Schedule(IScheduler scheduler)
    {
        var jobDetail = JobBuilder.Create<ProductSaleTimesCalJob>().WithDescription("ProductSaleTimesCalJob").Build();

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

        ProductSaleService productSaleService = new ProductSaleService(db);

        var productSaleListToUpdate = db.ProductSale.Where(e => e.UpdateTime > someTimeAgo).ToList();
        foreach (var productSale in productSaleListToUpdate)
        {
            long productSaleId = productSale.Id;
            productSaleService.UpdateProductSaleByLatestData(productSaleId);
        }

        return Task.CompletedTask;
    }
}