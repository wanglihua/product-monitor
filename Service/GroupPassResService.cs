using Core;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class GroupPassResService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
}