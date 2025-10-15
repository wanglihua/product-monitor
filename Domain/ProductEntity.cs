using Core;

namespace Domain;

public class ProductEntity : Entity
{
    /// <summary>
    /// 产品名称
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// 版本号
    /// </summary>
    public string Version { get; set; } = "";

    /// <summary>
    /// 更新基础
    /// </summary>
    public string VersionUpdateBase { get; set; } = "";

    /// <summary>
    /// 更新内容
    /// </summary>
    public string VersionUpdateDesc { get; set; } = "";

    /// <summary>
    /// 版本更新时间
    /// </summary>
    public DateTime VersionUpdateTime { get; set; }
}