using Core;
using Domain;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class ProductEvalService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;

    public ProductEvalService(Db db)
    {
        this.db = db;
    }

    /// <summary>
    /// 创建试用目录
    /// </summary>
    /// <param name="collegeId">学校</param>
    /// <param name="productId">产品</param>
    /// <param name="userId">创建者</param>
    /// <returns>返回创建的试用目录Id</returns>
    public long Create(long collegeId, long productId, long userId)
    {
        var productEval = db.Set<ProductEvalEntity>()
            .Where(e => e.CollegeId == collegeId && e.ProductId == productId)
            .Take(1)
            .SingleOrDefault();
        if (productEval == null)
        {
            throw new ValidationException($"学校#{collegeId}和产品#{productId}对应的试用目录已经存在!");
        }

        var college = db.Set<CollegeEntity>().Find(collegeId);
        if (college == null)
        {
            throw new ValidationException($"指定的学校#{collegeId}不存在!");
        }

        var product = db.Find<ProductEntity>(productId);
        if (product == null)
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

        db.Add<ProductEvalEntity>(productEval);

        db.SaveChanges();

        return productEval.Id;
    }

    /// <summary>
    /// 通过最新的明细数据等更新指定的试用目录
    /// </summary>
    /// <param name="productEvalId">试用目录id</param>
    public void UpdateProductEvalByLatestData(long productEvalId)
    {
        var productEval = db.Set<ProductEvalEntity>().Find(productEvalId);
        if (productEval == null)
        {
            throw new ValidationException($"指定的试用目录#{productEvalId}不存在!");
        }

        var productEvalEntryLatest = db.Set<ProductEvalEntryEntity>()
            .Where(e => e.ProductEvalId == productEvalId)
            .OrderByDescending(e => e.Id)
            .Take(1)
            .SingleOrDefault();

        if (productEvalEntryLatest == null)
        {
            throw new ValidationException($"指定的试用目录#{productEvalId}, 当前没有明细数据!");
        }

        productEval.TotalDays = productEvalEntryLatest.TotalDays; // 这个应该要根据  BeginDate 和  EndDate 计算出来, 把这个计算放在 Entry 里面
        productEval.BeginDate = productEvalEntryLatest.BeginDate;
        productEval.EndDate = productEvalEntryLatest.EndDate;

        int remainDays = (int)((productEvalEntryLatest.EndDate - productEvalEntryLatest.BeginDate).TotalDays);
        if (remainDays < 0)
        {
            remainDays = 0;
        }

        productEval.RemainDays = remainDays;

        // todo 还有这两个字段的更新
        // EvalTimes
        // ServiceTimes

        db.Update<ProductEvalEntity>(productEval);
        db.SaveChanges();
    }
}