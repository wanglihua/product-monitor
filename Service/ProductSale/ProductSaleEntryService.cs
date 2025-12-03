using Core;
using NLog;

namespace Service.ProductSale;

[Service(ServiceType.Scoped)]
public class ProductSaleEntryService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
}