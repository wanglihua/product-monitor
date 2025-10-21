
using Core;

namespace Domain;

/// <summary>
/// 院校目录
/// </summary>
public class CollegeEntity : Entity
{
    /// <summary>
    /// 院校标识码
    /// </summary>
    public string Code { get; set; } = ""; 

    /// <summary>
    /// 院校名称
    /// </summary>
    public string Name { get; set; } = ""; 

    /// <summary>
    /// 身份
    /// </summary>
    public string Province { get; set; } = ""; 

    /// <summary>
    /// 办学层次
    /// </summary>
    public string Level { get; set; } = ""; 

}