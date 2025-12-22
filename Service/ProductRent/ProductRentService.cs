using Core;
using Domain;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace Service.ProductRent;

[Service(ServiceType.Scoped)]
public class ProductRentService(
    Db db,
    UserProvinceService userProvinceService)
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
        var productRent = db.ProductRent.AsNoTracking()
            .Where(e => e.CollegeId == collegeId && e.ProductId == productId)
            .OrderBy(e => e.CollegeId)
            .Take(1)
            .SingleOrDefault();
        if (productRent is not null)
        {
            throw new ValidationException($"学校#{collegeId}和产品#{productId}对应的租赁目录已经存在!");
        }

        var college = db.College.Find(collegeId);
        if (college is null)
        {
            throw new ValidationException($"指定的学校#{collegeId}不存在!");
        }

        var product = db.Product.Find(productId);
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
    public void UpdateProductRentStatisticsData(long productRentId)
    {
        var productRent = db.ProductRent.Find(productRentId);
        if (productRent is null)
        {
            throw new ValidationException($"指定的租赁目录#{productRentId}不存在!");
        }

        var productRentEntryList = db.ProductRentEntry.AsNoTracking()
            .Where(e => e.ProductRentId == productRentId)
            .ToList();

        if (productRentEntryList.Count == 0)
        {
            throw new ValidationException($"指定的租赁目录#{productRentId}, 当前没有明细数据!");
        }

        DateTime beginDateAll = DateTime.MaxValue;
        DateTime endDateAll = DateTime.MinValue;
        int totalDaysAll = 0;
        int remainDaysAll = 0;

        foreach (var productRentEntry in productRentEntryList)
        {
            DateTime beginDate = productRentEntry.BeginDate.Date;
            if (beginDate < beginDateAll)
            {
                beginDateAll = beginDate;
            }

            DateTime endDate = productRentEntry.EndDate.Date;
            if (endDate > endDateAll)
            {
                endDateAll = endDate;
            }

            int totalDays = (int)(endDate - beginDate).TotalDays + 1;
            if (totalDays < 0)
            {
                totalDays = 0;
            }

            totalDaysAll += totalDays;

            int remainDays = (int)(endDate - DateTime.Today).TotalDays;
            if (remainDays < 0)
            {
                remainDays = 0;
            }

            remainDaysAll += remainDays;
        }

        productRent.BeginDate = beginDateAll;
        productRent.EndDate = endDateAll;
        productRent.TotalDays = totalDaysAll;
        productRent.RemainDays = remainDaysAll;

        // 两个次数计算的字段
        // RentTimes
        // ServiceTimes
        long productId = productRent.ProductId;
        long collegeId = productRent.CollegeId;

        int rentTimes = db.ProductRentEntry.Count(e => e.ProductRentId == productRentId);
        int serviceTimes = db.ProductService.Count(e => e.ProductId == productId && e.CollegeId == collegeId && e.ProjectType == ProjectType.Rent)
                           + db.ProductSpecialService.Count(e => e.ProductId == productId && e.CollegeId == collegeId && e.ProjectType == ProjectType.Rent);

        productRent.RentTimes = rentTimes;
        productRent.ServiceTimes = serviceTimes;

        db.SaveChanges();
    }

    public void UpdateProductRentStatisticsDataByProductInstall(long productInstallId)
    {
        var productInstall = db.ProductInstall.Find(productInstallId);
        if (productInstall is null)
        {
            throw new ValidationException($"指定的产品安装#{productInstallId}不存在！");
        }

        long collegeId = productInstall.CollegeId;
        long productId = productInstall.ProductId;
        var productRent = db.ProductRent.AsNoTracking()
            .Where(e => e.CollegeId == collegeId && e.ProductId == productId)
            .Take(1)
            .SingleOrDefault();

        if (productRent is not null)
        {
            long productRentId = productRent.Id;
            UpdateProductRentStatisticsData(productRentId);
        }
    }

    public void UpdateProductRentStatisticsDataByProductUpdate(long productUpdateId)
    {
        var productUpdate = db.ProductUpdate.Find(productUpdateId);
        if (productUpdate is null)
        {
            throw new ValidationException($"指定的产品更新#{productUpdateId}不存在！");
        }

        long collegeId = productUpdate.CollegeId;
        long productId = productUpdate.ProductId;
        var productRent = db.ProductRent.AsNoTracking()
            .Where(e => e.CollegeId == collegeId && e.ProductId == productId)
            .Take(1)
            .SingleOrDefault();

        if (productRent is not null)
        {
            long productRentId = productRent.Id;
            UpdateProductRentStatisticsData(productRentId);
        }
    }

    public void UpdateProductRentStatisticsDataByProductService(long productServiceId)
    {
        var productService = db.ProductService.Find(productServiceId);
        if (productService is null)
        {
            throw new ValidationException($"指定的售后维护#{productServiceId}不存在！");
        }

        long collegeId = productService.CollegeId;
        long productId = productService.ProductId;
        var productRent = db.ProductRent.AsNoTracking()
            .Where(e => e.CollegeId == collegeId && e.ProductId == productId)
            .Take(1)
            .SingleOrDefault();

        if (productRent is not null)
        {
            long productRentId = productRent.Id;
            UpdateProductRentStatisticsData(productRentId);
        }
    }

    public void UpdateProductRentStatisticsDataByProductSpecialService(long productSpecialServiceId)
    {
        var productSpecialService = db.ProductSpecialService.Find(productSpecialServiceId);
        if (productSpecialService is null)
        {
            throw new ValidationException($"指定的特殊售后#{productSpecialServiceId}不存在！");
        }

        long collegeId = productSpecialService.CollegeId;
        long productId = productSpecialService.ProductId;
        var productRent = db.ProductRent.AsNoTracking()
            .Where(e => e.CollegeId == collegeId && e.ProductId == productId)
            .Take(1)
            .SingleOrDefault();

        if (productRent is not null)
        {
            long productRentId = productRent.Id;
            UpdateProductRentStatisticsData(productRentId);
        }
    }

    public void CheckDataPrivilege(long productRentId, long loginUserId, bool isAdminLogin)
    {
        var productRent = db.ProductRent.Find(productRentId);
        if (productRent is null)
        {
            throw new ValidationException($"指定的租赁目录#{productRentId}不存在!");
        }

        long collegeId = productRent.CollegeId;
        var college = db.College.Find(collegeId);
        if (college is null)
        {
            throw new ValidationException($"指定学校#{collegeId}不存在!");
        }

        if (isAdminLogin == false)
        {
            var userProvinceList = userProvinceService.GetUserProvinceList(loginUserId);
            if (userProvinceList.Contains(college.Province) == false)
            {
                throw new ValidationException($"院校 {college.Name} 不在用户负责区域！");
            }
        }
    }
}