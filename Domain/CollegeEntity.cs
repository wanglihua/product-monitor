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
    /// 省份
    /// </summary>
    public ProvinceType Province { get; set; } = ProvinceType.None;

    /// <summary>
    /// 办学层次
    /// </summary>
    public CollegeLevelType Level { get; set; } = CollegeLevelType.None;
}