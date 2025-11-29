using Core;
using Domain;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class ProductRentService(Db db)
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// 创建租赁目录
    /// </summary>
    /// <param name="collegeId">学校</param>
    /// <param name="productId">产品</param>
    /// <param name="userId">创建者</param>
    /// <returns>返回创建的租赁目录Id</returns>
    public long Create(long collegeId, long productId, long userId)
    {
        var productRent = db.Set<ProductRentEntity>()
            .Where(e => e.CollegeId == collegeId && e.ProductId == productId)
            .OrderBy(e => e.CollegeId)
            .Take(1)
            .SingleOrDefault();
        if (productRent is not null)
        {
            throw new ValidationException($"学校#{collegeId}和产品#{productId}对应的租赁目录已经存在!");
        }

        var college = db.Set<CollegeEntity>().Find(collegeId);
        if (college is null)
        {
            throw new ValidationException($"指定的学校#{collegeId}不存在!");
        }

        var product = db.Find<ProductEntity>(productId);
        if (product is null)
        {
            throw new ValidationException($"指定的产品#{productId}不存在!");
        }

        productRent = new ProductRentEntity {
            CollegeId = collegeId,
            ProductId = productId,

            // 后面会更新修改下面的这几个值
            RentTimes = 0,
            ServiceTimes = 0,
            TotalDays = 0,
            RemainDays = 0,
            BeginDate = DateTime.Today,
            EndDate = DateTime.Today,
            Description = "",
        };

        db.ProductRent.Add(productRent);

        db.SaveChanges();

        return productRent.Id;
    }

    /// <summary>
    /// 通过最新的明细数据等更新指定的租赁目录
    /// </summary>
    /// <param name="productRentId">租赁目录id</param>
    public void UpdateProductRentByLatestData(long productRentId)
    {
        var productRent = db.Set<ProductRentEntity>().Find(productRentId);
        if (productRent is null)
        {
            throw new ValidationException($"指定的租赁目录#{productRentId}不存在!");
        }

        var productRentEntryLatest = db.Set<ProductRentEntryEntity>()
            .Where(e => e.ProductRentId == productRentId)
            .OrderByDescending(e => e.Id)
            .Take(1)
            .SingleOrDefault();

        if (productRentEntryLatest is null)
        {
            throw new ValidationException($"指定的租赁目录#{productRentId}, 当前没有明细数据!");
        }

        productRent.TotalDays = productRentEntryLatest.TotalDays; // 这个应该要根据  BeginDate 和  EndDate 计算出来, 把这个计算放在 Entry 里面
        productRent.BeginDate = productRentEntryLatest.BeginDate;
        productRent.EndDate = productRentEntryLatest.EndDate;

        int remainDays = (int)((productRentEntryLatest.EndDate - productRentEntryLatest.BeginDate).TotalDays);
        if (remainDays < 0)
        {
            remainDays = 0;
        }

        productRent.RemainDays = remainDays;

        // todo 还有这两个字段的更新
        // RentTimes
        // ServiceTimes

        db.ProductRent.Update(productRent);
        db.SaveChanges();
    }
}