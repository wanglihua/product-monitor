using Core;
using NLog;

namespace Service.College;

[Service(ServiceType.Scoped)]
public class CollegeService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

}