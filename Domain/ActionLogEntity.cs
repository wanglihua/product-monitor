using Core;

namespace Domain;

/// <summary>
/// 操作日志
/// </summary>
public class ActionLogEntity : EntityNoUpdate
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public ActionLogType Type { get; set; } = ActionLogType.None;

    /// <summary>
    /// 操作用户
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 操作描述
    /// </summary>
    public string ActionDesc { get; set; } = "";
}