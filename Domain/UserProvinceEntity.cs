using Core;

namespace Domain;

/// <summary>
/// 用户区域设置
/// </summary>
public class UserProvinceEntity : EntityNoUpdate
{
    /// <summary>
    /// 用户
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    ///  省份
    /// </summary>
    public ProvinceType ProvinceType { get; set; } = ProvinceType.None;
}