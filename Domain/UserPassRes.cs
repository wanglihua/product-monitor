namespace Domain;

/// <summary>
/// 用户资源授权关联
/// </summary>
public class UserPassRes
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