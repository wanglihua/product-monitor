using Core;
using NLog;

namespace Service.ProductService;

[Service(ServiceType.Scoped)]
public class ProductServiceService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
}