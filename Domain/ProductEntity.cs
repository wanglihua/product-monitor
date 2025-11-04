using Core;

namespace Domain;

public class ProductEntity : Entity
{
    /// <summary>
    /// 产品名称
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// 产品英文名
    /// </summary>
    public string EnglishName { get; set; } = "";

    /// <summary>
    /// 产品英文名
    /// </summary>
    public int VersionCount { get; set; }

    /// <summary>
    /// 最新版本号
    /// </summary>
    public string LatestVersion { get; set; } = "";

    /// <summary>
    /// 版本更新时间
    /// </summary>
    public DateTime? VersionUpdateTime { get; set; }
}