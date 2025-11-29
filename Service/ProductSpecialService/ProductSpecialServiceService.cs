using Core;
using NLog;

namespace Service.ProductSpecialService;

[Service(ServiceType.Scoped)]
public class ProductSpecialServiceService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
}