using Core;

namespace Domain;

/// <summary>
/// 安装监测
/// </summary>
public class ProductInstallMonitorEntity : Entity
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
    /// 省份
    /// </summary>
    public ProvinceType ProvinceType { get; set; } = ProvinceType.None;

    /// <summary>
    /// CPU序列号
    /// </summary>
    public string CpuSerialNo { get; set; } = "";

    /// <summary>
    /// 网卡硬件地址
    /// </summary>
    public string NetMacAddress { get; set; } = "";

    /// <summary>
    /// 任务数量
    /// </summary>
    public int TaskCount { get; set; }

    /// <summary>
    /// 教师数量
    /// </summary>
    public int TeacherCount { get; set; }
}