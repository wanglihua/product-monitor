using Core;
using Domain;
using NLog;

namespace Service.ProductSpecialService;

[Service(ServiceType.Scoped)]
public class ProductSpecialServiceService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;

    public ProductSpecialServiceService(Db db)
    {
        this.db = db;
    }
}