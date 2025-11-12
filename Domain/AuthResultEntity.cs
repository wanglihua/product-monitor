using Core;

namespace Domain;

/// <summary>
/// 
/// </summary>
public class AuthResultEntity : Entity
{
    /// <summary>
    /// 
    /// </summary>
    public long PassResCateId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string PassResCateName { get; set; } = ""; 

    /// <summary>
    /// 
    /// </summary>
    public long PassResId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string PassResName { get; set; } = ""; 

    /// <summary>
    /// 
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string LoginName { get; set; } = ""; 

    /// <summary>
    /// 
    /// </summary>
    public string UserName { get; set; } = ""; 

    /// <summary>
    /// 
    /// </summary>
    public long? GroupId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string GroupName { get; set; } = ""; 

}