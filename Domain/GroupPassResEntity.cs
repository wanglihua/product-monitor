using Core;

namespace Domain;

/// <summary>
/// 群组授权
/// </summary>
public class GroupPassResEntity : EntityNoUpdate
{
    /// <summary>
    /// 群组
    /// </summary>
    public long GroupId { get; set; }

    /// <summary>
    /// 资源
    /// </summary>
    public long PassResId { get; set; }
}