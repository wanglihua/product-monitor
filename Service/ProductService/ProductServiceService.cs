using Core;
using Domain;
using NLog;

namespace Service.ProductService;

[Service(ServiceType.Scoped)]
public class ProductServiceService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;

    public ProductServiceService(Db db)
    {
        this.db = db;
    }
}