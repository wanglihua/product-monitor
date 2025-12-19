using Core;
using Domain;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace Service.ProductEval;

[Service(ServiceType.Scoped)]
public class ProductEvalService(
    Db db,
    UserProvinceService userProvinceService)
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// 创建试用目录
    /// </summary>
    /// <param name="collegeId">学校</param>
    /// <param name="productId">产品</param>
    /// <param name="userId">创建者</param>
    /// <returns>返回创建的试用目录Id</returns>
    public long Create(long collegeId, long productId, long userId)
    {
        var productEval = db.ProductEval.AsNoTracking()
            .Where(e => e.CollegeId == collegeId && e.ProductId == productId)
            .OrderBy(e => e.CollegeId)
            .Take(1)
            .SingleOrDefault();
        if (productEval is not null)
        {
            throw new ValidationException($"学校#{collegeId}和产品#{productId}对应的试用目录已经存在!");
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

        productEval = new ProductEvalEntity {
            CollegeId = collegeId,
            ProductId = productId,

            // 后面会更新修改下面的这几个值
            EvalTimes = 0,
            ServiceTimes = 0,
            TotalDays = 0,
            RemainDays = 0,
            BeginDate = DateTime.Today,
            EndDate = DateTime.Today,
            Description = "",
        };

        db.ProductEval.Add(productEval);

        db.SaveChanges();

        return productEval.Id;
    }

    /// <summary>
    /// 通过最新的明细数据等更新指定的试用目录
    /// </summary>
    /// <param name="productEvalId">试用目录id</param>
    public void UpdateProductEvalStatisticsData(long productEvalId)
    {
        var productEval = db.ProductEval.Find(productEvalId);
        if (productEval is null)
        {
            throw new ValidationException($"指定的试用目录#{productEvalId}不存在!");
        }

        var productEvalEntryLatest = db.ProductEvalEntry.AsNoTracking()
            .Where(e => e.ProductEvalId == productEvalId)
            .OrderByDescending(e => e.Id)
            .Take(1)
            .SingleOrDefault();

        if (productEvalEntryLatest is null)
        {
            throw new ValidationException($"指定的试用目录#{productEvalId}, 当前没有明细数据!");
        }

        productEval.TotalDays = productEvalEntryLatest.TotalDays; // 这个应该要根据  BeginDate 和  EndDate 计算出来, 把这个计算放在 Entry 里面
        productEval.BeginDate = productEvalEntryLatest.BeginDate;
        productEval.EndDate = productEvalEntryLatest.EndDate;

        int remainDays = (int)((productEvalEntryLatest.EndDate - DateTime.Today).TotalDays);
        if (remainDays < 0)
        {
            remainDays = 0;
        }

        productEval.RemainDays = remainDays;

        // 两个次数计算的字段
        long productId = productEval.ProductId;
        long collegeId = productEval.CollegeId;

        int evalTimes = db.ProductEvalEntry.Count(e => e.ProductEvalId == productEvalId);
        int serviceTimes = db.ProductService.Count(e => e.ProductId == productId && e.CollegeId == collegeId && e.ProjectType == ProjectType.Eval)
                           + db.ProductSpecialService.Count(e => e.ProductId == productId && e.CollegeId == collegeId && e.ProjectType == ProjectType.Eval);

        productEval.EvalTimes = evalTimes;
        productEval.ServiceTimes = serviceTimes;

        db.SaveChanges();
    }

    public void CheckDataPrivilege(long productEvalId, long loginUserId, bool isAdminLogin)
    {
        var productEval = db.ProductEval.Find(productEvalId);
        if (productEval is null)
        {
            throw new ValidationException($"指定的试用目录#{productEvalId}不存在!");
        }

        long collegeId = productEval.CollegeId;
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