using Core;
using Domain;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class ProductSaleService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;

    public ProductSaleService(Db db)
    {
        this.db = db;
    }

    /// <summary>
    /// 创建销售目录
    /// </summary>
    /// <param name="collegeId">学校</param>
    /// <param name="productId">产品</param>
    /// <param name="userId">创建者</param>
    /// <returns>返回创建的销售目录Id</returns>
    public long Create(long collegeId, long productId, long userId)
    {
        var productSale = db.Set<ProductSaleEntity>()
            .Where(e => e.CollegeId == collegeId && e.ProductId == productId)
            .OrderBy(e => e.CollegeId)
            .Take(1)
            .SingleOrDefault();
        if (productSale is not null)
        {
            throw new ValidationException($"学校#{collegeId}和产品#{productId}对应的销售目录已经存在!");
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

        productSale = new ProductSaleEntity {
            CollegeId = collegeId,
            ProductId = productId,

            // 后面会更新修改下面的这几个值
            SaleTimes = 0,
            InstallTimes = 0,
            UpdateTimes = 0,
            ServiceTimes = 0,
            Version = "",
            SaleDate = DateTime.Today,
            ServiceExpireDate = DateTime.Today,
            Description = "",
        };

        db.Add<ProductSaleEntity>(productSale);

        db.SaveChanges();

        return productSale.Id;
    }

    /// <summary>
    /// 通过最新的明细数据等更新指定的销售目录
    /// </summary>
    /// <param name="productSaleId">销售目录id</param>
    public void UpdateProductSaleByLatestData(long productSaleId)
    {
        var productSale = db.Set<ProductSaleEntity>().Find(productSaleId);
        if (productSale is null)
        {
            throw new ValidationException($"指定的销售目录#{productSaleId}不存在!");
        }

        var productSaleEntryLatest = db.Set<ProductSaleEntryEntity>()
            .Where(e => e.ProductSaleId == productSaleId)
            .OrderByDescending(e => e.Id)
            .Take(1)
            .SingleOrDefault();

        if (productSaleEntryLatest is null)
        {
            throw new ValidationException($"指定的销售目录#{productSaleId}, 当前没有明细数据!");
        }

        productSale.Version = productSaleEntryLatest.Version;
        productSale.SaleDate = productSaleEntryLatest.SaleDate;
        productSale.ServiceExpireDate = productSaleEntryLatest.ServiceExpireDate;
        productSale.Description = productSaleEntryLatest.Description;

        // todo 还有这四个字段的更新
        // SaleTimes
        // InstallTimes
        // UpdateTimes
        // ServiceTimes

        db.Update<ProductSaleEntity>(productSale);
        db.SaveChanges();
    }
}