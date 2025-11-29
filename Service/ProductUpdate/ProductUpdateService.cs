using Core;
using NLog;

namespace Service.ProductUpdate;

[Service(ServiceType.Scoped)]
public class ProductUpdateService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
}