using Core;

namespace Domain;

/// <summary>
/// 网站监测详情
/// </summary>
public class ProductUrlMonitorEntryEntity : Entity
{
    /// <summary>
    /// 网站监测
    /// </summary>
    public long ProductUrlMonitorId { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    public string LoginUser { get; set; } = "";

    /// <summary>
    /// 密码
    /// </summary>
    public string LoginPassword { get; set; } = "";

    /// <summary>
    /// 使用地址
    /// </summary>
    public string IpAddress { get; set; } = "";

    /// <summary>
    /// 省份
    /// </summary>
    public ProvinceType ProvinceType { get; set; } = ProvinceType.None;
}