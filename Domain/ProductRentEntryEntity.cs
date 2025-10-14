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
    /// 存的是日期
    /// </summary>
    public DateTime BeginDate { get; set; }

    /// <summary>
    /// 结束时间
    /// 存的是日期
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Description { get; set; } = "";
}