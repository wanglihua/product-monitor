using Core;
using NLog;

namespace Service.ProductSale;

[Service(ServiceType.Scoped)]
public class ProductSaleExcelService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
}