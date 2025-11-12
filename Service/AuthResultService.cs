using Core;
using Domain;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class AuthResultService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;

    public AuthResultService(Db db)
    {
        this.db = db;
    }
}