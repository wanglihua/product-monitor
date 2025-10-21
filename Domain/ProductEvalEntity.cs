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
    /// 销售次数
    /// </summary>
    public int SaleTimes { get; set; }

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
    /// </summary>
    public DateTime BeginDate { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Description { get; set; } = ""; 

}