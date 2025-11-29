using Core;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class ProductInstallMonitorEntryService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
}