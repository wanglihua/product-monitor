using Core;
using Domain;
using NLog;

namespace Service.ProductUpdate;

[Service(ServiceType.Scoped)]
public class ProductUpdateService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;

    public ProductUpdateService(Db db)
    {
        this.db = db;
    }
}