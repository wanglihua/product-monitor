using Core;

namespace Domain;

/// <summary>
/// 试用目录
/// </summary>
public class ProductEvalEntity : Entity
{
    /// <summary>
    /// 学校
    /// </summary>
    public long CollegeId { get; set; }

    /// <summary>
    /// 产品
    /// </summary>
    public long ProductId { get; set; }

    /// <summary>
    /// 试用次数
    /// </summary>
    public int EvalTimes { get; set; }

    /// <summary>
    /// 售后次数
    /// </summary>
    public int ServiceTimes { get; set; }

    /// <summary>
    /// 累计天数
    /// </summary>
    public int TotalDays { get; set; }

    /// <summary>
    /// 剩余天数
    /// </summary>
    public int RemainDays { get; set; }

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