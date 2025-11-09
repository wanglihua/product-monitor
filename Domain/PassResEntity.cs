using Core;

namespace Domain;

/// <summary>
/// 授权资源
/// </summary>
public class PassResEntity : EntityAssignedId
{
    /// <summary>
    /// 资源类别
    /// </summary>
    public long CateId { get; set; }

    /// <summary>
    /// 资源名称
    /// </summary>
    public string Name { get; set; } = "";
}