using Core;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class ProductRentEntryService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
}