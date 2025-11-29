using Core;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class ExceptionMonitorService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
}