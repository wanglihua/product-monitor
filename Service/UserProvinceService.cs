using Core;
using Domain;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class UserProvinceService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;

    public UserProvinceService(Db db)
    {
        this.db = db;
    }
}