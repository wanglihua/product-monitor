using Core;

namespace Domain;

/// <summary>
/// 用户授权
/// </summary>
public class UserPassResEntity : EntityNoUpdate
{
    /// <summary>
    /// 用户
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 授权资源
    /// </summary>
    public long PassResId { get; set; }
}