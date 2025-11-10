using Core;
using Domain;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class UserPassResService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;

    public UserPassResService(Db db)
    {
        this.db = db;
    }
}