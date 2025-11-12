using Core;

namespace Domain;

/// <summary>
/// 授权结果
/// </summary>
public class AuthResultEntity : EntityNoUpdate
{
    /// <summary>
    /// 资源类别
    /// </summary>
    public long PassResCateId { get; set; }

    /// <summary>
    /// 资源类别名称
    /// </summary>
    public string PassResCateName { get; set; } = ""; 

    /// <summary>
    /// 资源Id
    /// </summary>
    public long PassResId { get; set; }

    /// <summary>
    /// 资源名称
    /// </summary>
    public string PassResName { get; set; } = ""; 

    /// <summary>
    /// 用户Id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 登录名
    /// </summary>
    public string LoginName { get; set; } = ""; 

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = ""; 

    /// <summary>
    /// 群组Id
    /// </summary>
    public long? GroupId { get; set; }

    /// <summary>
    /// 群组名称
    /// </summary>
    public string GroupName { get; set; } = ""; 

}