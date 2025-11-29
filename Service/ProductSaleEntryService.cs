using Core;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class ProductSaleEntryService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
}