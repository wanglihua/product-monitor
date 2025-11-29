using Core;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class ProductUrlMonitorEntryService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
}