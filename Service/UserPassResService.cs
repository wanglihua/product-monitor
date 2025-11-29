using Core;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class UserPassResService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
}