using Core;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class ProductInstallMonitorService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
}