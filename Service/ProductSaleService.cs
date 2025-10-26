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
    /// <returns></returns>
    public long Create(long collegeId, long productId, long userId)
    {
        return 0;
    }

    /// <summary>
    /// 通过最新的明细数据等更新指定的销售目录
    /// </summary>
    /// <param name="productSaleId">销售目录id</param>
    public void UpdateByLatestData(long productSaleId)
    {
    }
}