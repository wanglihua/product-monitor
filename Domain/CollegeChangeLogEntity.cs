using Core;

namespace Domain;

/// <summary>
/// 院校更新记录
/// </summary>
public class CollegeChangeLogEntity : Entity
{
    /// <summary>
    /// 院校
    /// </summary>
    public long CollegeId { get; set; }

    /// <summary>
    /// 曾用名
    /// </summary>
    public string FormerName { get; set; } = "";

    /// <summary>
    /// 更新描述
    /// </summary>
    public string ChangeDesc { get; set; } = "";
}