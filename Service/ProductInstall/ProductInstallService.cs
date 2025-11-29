using Core;
using NLog;

namespace Service.ProductInstall;

[Service(ServiceType.Scoped)]
public class ProductInstallService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
}