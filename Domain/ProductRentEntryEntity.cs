using Core;

namespace Domain;

/// <summary>
/// 租赁目录明细
/// </summary>
public class ProductRentEntryEntity : Entity
{
    /// <summary>
    /// 租赁目录
    /// </summary>
    public long ProductRentId { get; set; }

    /// <summary>
    /// 累计天数
    /// </summary>
    public int TotalDays { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime BeginDate { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndDate { get; set; }

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