using Core;

namespace Domain;

/// <summary>
/// 异常检测
/// </summary>
public class ExceptionMonitorEntity : Entity
{
    /// <summary>
    /// 产品
    /// </summary>
    public long ProductId { get; set; }

    /// <summary>
    /// 使用IP
    /// </summary>
    public string IpAddress { get; set; } = "";

    /// <summary>
    /// 使用省份
    /// </summary>
    public ProvinceType ProvinceType { get; set; } = ProvinceType.None;

    /// <summary>
    /// 使用端口
    /// </summary>
    public int Port { get; set; }
}