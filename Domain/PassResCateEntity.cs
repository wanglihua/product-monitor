using Core;

namespace Domain;

/// <summary>
/// 授权资源类别
/// </summary>
public class PassResCateEntity : EntityAssignedId
{
    /// <summary>
    /// 类别名称
    /// </summary>
    public string Name { get; set; } = "";
}