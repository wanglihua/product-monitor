using Core;

namespace Domain;

/// <summary>
/// 用户
/// </summary>
public class UserEntity : Entity
{
    /// <summary>
    /// 登录名
    /// </summary>
    public string LoginName { get; set; } = "";

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = "";

    /// <summary>
    /// 密码, 加密存储
    /// </summary>
    public string Password { get; set; } = "";

    /// <summary>
    /// 加密算法的盐
    /// </summary>
    public string Salt { get; set; } = "";

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTime? LastLoginTime { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public UserStatus Status { get; set; } = UserStatus.None;
}