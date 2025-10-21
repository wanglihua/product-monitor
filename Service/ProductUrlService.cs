using Core;
using Domain;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class ProductUrlService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;

    public ProductUrlService(Db db)
    {
        this.db = db;
    }
}