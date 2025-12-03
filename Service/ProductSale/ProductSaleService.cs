using Core;
using Domain;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace Service.ProductSale;

[Service(ServiceType.Scoped)]
public class ProductSaleService(Db db)
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// 创建销售目录
    /// </summary>
    /// <param name="collegeId">学校</param>
    /// <param name="productId">产品</param>
    /// <param name="userId">创建者</param>
    /// <returns>返回创建的销售目录Id</returns>
    public long Create(long collegeId, long productId, long userId)
    {
        var productSale = db.ProductSale.AsNoTracking()
            .Where(e => e.CollegeId == collegeId && e.ProductId == productId)
            .OrderBy(e => e.CollegeId)
            .Take(1)
            .SingleOrDefault();
        if (productSale is not null)
        {
            throw new ValidationException($"学校#{collegeId}和产品#{productId}对应的销售目录已经存在!");
        }

        var college = db.College.Find(collegeId);
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

        db.ProductSale.Add(productSale);

        db.SaveChanges();

        return productSale.Id;
    }

    /// <summary>
    /// 通过最新的明细数据等更新指定的销售目录
    /// </summary>
    /// <param name="productSaleId">销售目录id</param>
    public void UpdateProductSaleByLatestData(long productSaleId)
    {
        var productSale = db.ProductSale.Find(productSaleId);
        if (productSale is null)
        {
            throw new ValidationException($"指定的销售目录#{productSaleId}不存在!");
        }

        var productSaleEntryLatest = db.ProductSaleEntry.AsNoTracking()
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

        // 四个次数计算的字段
        long productId = productSale.ProductId;
        long collegeId = productSale.CollegeId;

        int saleTimes = db.ProductSaleEntry.Count(e => e.ProductSaleId == productSaleId);
        int installTimes = db.ProductInstall.Count(e => e.ProductId == productId && e.CollegeId == collegeId);
        int updateTimes = db.ProductUpdate.Count(e => e.ProductId == productId && e.CollegeId == collegeId);
        int serviceTimes = db.ProductService.Count(e => e.ProductId == productId && e.CollegeId == collegeId && e.ProjectType == ProjectType.Sale)
                           + db.ProductSpecialService.Count(e => e.ProductId == productId && e.CollegeId == collegeId /* && e.ProjectType == ProjectType.Sale */);

        productSale.SaleTimes = saleTimes;
        productSale.InstallTimes = installTimes;
        productSale.UpdateTimes = updateTimes;
        productSale.ServiceTimes = serviceTimes;

        db.ProductSale.Update(productSale);
        db.SaveChanges();
    }
}