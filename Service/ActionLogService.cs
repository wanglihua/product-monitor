using Core;
using Domain;

namespace Service;

[Service(ServiceType.Scoped)]
public class ActionLogService
{
    private readonly Db db;

    public ActionLogService(Db db)
    {
        this.db = db;
    }

    public void Add(long userId, ActionLogType type)
    {
        Add(userId, type, string.Empty);
    }

    public void Add(long userId, ActionLogType type, string actionDesc)
    {
        var actionLog = new ActionLogEntity {
            UserId = userId,
            Type = ActionLogType.UserLogin,
            ActionDesc = actionDesc,
        };

        db.Add<ActionLogEntity>(actionLog);

        db.SaveChanges();
    }
}