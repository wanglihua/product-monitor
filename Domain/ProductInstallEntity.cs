using Core;

namespace Domain;

/// <summary>
/// 产品安装
/// </summary>
public class ProductInstallEntity : Entity
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
    /// 销售版本
    /// </summary>
    public string VersionSale { get; set; } = "";

    /// <summary>
    /// 安装版本
    /// </summary>
    public string VersionInstall { get; set; } = "";

    /// <summary>
    /// 安装时间
    /// </summary>
    public DateTime InstallDate { get; set; }

    /// <summary>
    /// 更新网址
    /// </summary>
    public string UpdateUrl { get; set; } = "";

    /// <summary>
    /// 系统账号
    /// </summary>
    public string LoginUser { get; set; } = "";

    /// <summary>
    /// 系统密码
    /// </summary>
    public string LoginPassword { get; set; } = "";

    /// <summary>
    /// 联系教师
    /// </summary>
    public string ContactTeacher { get; set; } = "";

    /// <summary>
    /// 联系电话
    /// </summary>
    public string ContactMobile { get; set; } = "";

    /// <summary>
    /// 其他备注
    /// </summary>
    public string Description { get; set; } = "";
}