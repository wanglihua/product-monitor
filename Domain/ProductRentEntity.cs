using Core;

namespace Domain;

/// <summary>
/// 租赁目录
/// </summary>
public class ProductRentEntity : Entity
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
    /// 租赁次数
    /// </summary>
    public int RentTimes { get; set; }

    /// <summary>
    /// 租赁次数
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