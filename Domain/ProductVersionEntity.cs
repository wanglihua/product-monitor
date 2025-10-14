using Core;

namespace Domain;

public class ProductVersionEntity : Entity
{
    public long ProductId { get; set; }

    /// <summary>
    /// 主版本号
    /// </summary>
    public int Major { get; set; }

    /// <summary>
    /// 此版本号
    /// </summary>
    public int Minor { get; set; }

    /// <summary>
    /// 修订号
    /// </summary>
    public int Revision { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; } = "";
}