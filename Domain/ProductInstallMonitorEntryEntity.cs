using Core;

namespace Domain;

/// <summary>
/// 院校使用详情
/// </summary>
public class ProductInstallMonitorEntryEntity : Entity
{
    /// <summary>
    /// 安装监测
    /// </summary>
    public long ProductMonitorId { get; set; }

    /// <summary>
    /// 使用账号
    /// </summary>
    public string LoginUser { get; set; } = "";

    /// <summary>
    /// 账号分类
    /// </summary>
    public AccountType AccountType { get; set; } = AccountType.None;

    /// <summary>
    /// 使用IP
    /// </summary>
    public string IpAddress { get; set; } = "";

    /// <summary>
    /// 使用省份
    /// </summary>
    public ProvinceType ProvinceType { get; set; } = ProvinceType.None;
}