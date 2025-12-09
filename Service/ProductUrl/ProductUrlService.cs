using Core;
using NLog;

namespace Service.ProductUrl;

[Service(ServiceType.Scoped)]
public class ProductUrlService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
}