using Core;

namespace Domain;

/// <summary>
/// 销售目录明细
/// </summary>
public class ProductSaleEntryEntity : Entity
{
    /// <summary>
    /// 销售目录 
    /// </summary>
    public long ProductSaleId { get; set; }

    /// <summary>
    /// 版本号
    /// </summary>
    public string Version { get; set; } = "";

    /// <summary>
    /// 销售时间
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// 服务到期时间
    /// </summary>
    public DateTime ServiceExpireDate { get; set; }

    /// <summary>
    /// 客户经理
    /// </summary>
    public string SalesManager { get; set; } = "";

    /// <summary>
    /// 联系老师
    /// </summary>
    public string ContactTeacher { get; set; } = "";

    /// <summary>
    /// 联系电话
    /// </summary>
    public string ContactMobile { get; set; } = "";

    /// <summary>
    /// 备注
    /// </summary>
    public string Description { get; set; } = "";

    /// <summary>
    /// 记录人
    /// </summary>
    public long UserId { get; set; }
}