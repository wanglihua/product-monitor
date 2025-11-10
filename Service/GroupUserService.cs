using Core;
using Domain;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class GroupUserService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;

    public GroupUserService(Db db)
    {
        this.db = db;
    }
}