using Core;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class GroupService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
}