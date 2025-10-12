using Core;

namespace Domain;

/// <summary>
/// 销售目录
/// </summary>
public class ProductSaleEntity : Entity
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
    /// 销售次数
    /// </summary>
    public int SaleTimes { get; set; }

    /// <summary>
    /// 安装次数
    /// </summary>
    public int InstallTimes { get; set; }

    /// <summary>
    /// 更新次数
    /// </summary>
    public int UpdateTimes { get; set; }

    /// <summary>
    /// 售后次数
    /// </summary>
    public int ServiceTime { get; set; }

    /// <summary>
    /// 版本号
    /// </summary>
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// 销售时间
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// 服务到期时间
    /// </summary>
    public DateTime ServiceExpireDate { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Description { get; set; } = string.Empty;
}