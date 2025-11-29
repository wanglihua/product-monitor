using Core;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class GroupUserService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
}