using Core;
using Domain;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class ProductEvalEntryService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;

    public ProductEvalEntryService(Db db)
    {
        this.db = db;
    }
}