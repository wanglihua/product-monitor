using Core;

namespace Domain;

public class ActionLogEntity : EntityNoUpdate
{
    public ActionLogType Type { get; set; } = ActionLogType.None;

    public long UserId { get; set; }

    public string ActionDesc { get; set; } = string.Empty;
}