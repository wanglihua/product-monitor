using Core;
using Domain;
using NLog;
using Quartz;
using Service.ProductEval;
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

        var db = new Db();
        var userProvinceService = new UserProvinceService(db);

        // 销售目录
        var productSaleService = new ProductSaleService(db, userProvinceService);

        var productSaleListToUpdate = db.ProductSale.Where(e => e.UpdateTime > someTimeAgo).ToList();
        foreach (var productSale in productSaleListToUpdate)
        {
            long productSaleId = productSale.Id;
            productSaleService.UpdateProductSaleByLatestData(productSaleId);
        }

        // 租赁目录
        var productRentService = new ProductRentService(db, userProvinceService);
        var productRentListToUpdate = db.ProductRent.Where(e => e.UpdateTime > someTimeAgo).ToList();
        foreach (var productRent in productRentListToUpdate)
        {
            long productRentId = productRent.Id;
            productRentService.UpdateProductRentByLatestData(productRentId);
        }

        // 试用目录
        var productEvalService = new ProductEvalService(db, userProvinceService);
        var productEvalListToUpdate = db.ProductEval.Where(e => e.UpdateTime > someTimeAgo).ToList();
        foreach (var productEval in productEvalListToUpdate)
        {
            long productEvalId = productEval.Id;
            productEvalService.UpdateProductEvalByLatestData(productEvalId);
        }

        return Task.CompletedTask;
    }
}