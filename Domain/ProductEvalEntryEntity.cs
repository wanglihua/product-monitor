using Core;

namespace Domain;

/// <summary>
/// 试用目录明细
/// </summary>
public class ProductEvalEntryEntity : Entity
{
    /// <summary>
    /// 试用目录
    /// </summary>
    public long ProductEvalId { get; set; }

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
    public string Description { get; set; } = string.Empty;
}