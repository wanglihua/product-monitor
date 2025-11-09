using Core;
using Domain;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class GroupService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;

    public GroupService(Db db)
    {
        this.db = db;
    }
}