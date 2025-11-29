using Core;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class ProductUrlService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
}