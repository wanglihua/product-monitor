using Core;

namespace Domain;

/// <summary>
/// 群组成员
/// </summary>
public class GroupUserEntity : EntityNoUpdate
{
    /// <summary>
    /// 群组
    /// </summary>
    public long GroupId { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    public long UserId { get; set; }
}