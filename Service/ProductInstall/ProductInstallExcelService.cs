using Core;
using NLog;

namespace Service.ProductInstall;

[Service(ServiceType.Scoped)]
public class ProductInstallExcelService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    public byte[] GetExcelContent(IList<ProductInstallExcelExportListItem> productInstallList)
    {
        return Array.Empty<byte>();
    }
}