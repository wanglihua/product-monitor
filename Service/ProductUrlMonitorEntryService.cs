using Core;
using Domain;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class ProductUrlMonitorEntryService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;

    public ProductUrlMonitorEntryService(Db db)
    {
        this.db = db;
    }
}