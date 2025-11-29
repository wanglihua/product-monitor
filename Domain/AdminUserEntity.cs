using Core;

namespace Domain;

/// <summary>
/// 管理员用户
/// </summary>
public class AdminUserEntity : EntityNoUpdate
{
    /// <summary>
    /// 用户
    /// </summary>
    public long UserId { get; set; }
}