using Core;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class ProductUrlMonitorService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
}