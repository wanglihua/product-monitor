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

    /// <summary>
    /// 使用省份
    /// </summary>
    public string Province { get; set; } = "";

    /// <summary>
    /// CPU序列号
    /// </summary>
    public string CpuSerialNo { get; set; } = "";

    /// <summary>
    /// 网卡硬件地址
    /// </summary>
    public string NetMacAddress { get; set; } = "";

    /// <summary>
    /// 使用时间
    /// </summary>
    public DateTime UseTime { get; set; }
}