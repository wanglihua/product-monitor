using Core;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class AuthResultService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
}