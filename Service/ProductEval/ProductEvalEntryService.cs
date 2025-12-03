using Core;
using NLog;

namespace Service.ProductEval;

[Service(ServiceType.Scoped)]
public class ProductEvalEntryService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
}