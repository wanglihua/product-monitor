namespace Domain;

/// <summary>
/// 授权结果
/// </summary>
public class AuthResult
{
    /// <summary>
    /// 用户
    /// </summary>
    public UserEntity User { get; set; } = new();

    /// <summary>
    /// 群组
    /// </summary>
    public GroupEntity? Group { get; set; }

    /// <summary>
    /// 授权资源类别
    /// </summary>
    public PassResCateEntity PassResCate { get; set; } = new();

    /// <summary>
    /// 授权资源
    /// </summary>
    public PassResEntity PassRes { get; set; } = new();
}