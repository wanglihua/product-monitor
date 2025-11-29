using Core;
using Domain;

namespace Service;

[Service(ServiceType.Scoped)]
public class ActionLogService(Db db)
{
    public void Add(long userId, ActionLogType type)
    {
        Add(userId, type, string.Empty);
    }

    public void Add(long userId, ActionLogType type, string actionDesc)
    {
        var actionLog = new ActionLogEntity {
            UserId = userId,
            Type = type,
            ActionDesc = actionDesc,
        };

        db.ActionLog.Add(actionLog);

        db.SaveChanges();
    }
}