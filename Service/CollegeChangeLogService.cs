using Core;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class CollegeChangeLogService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
}