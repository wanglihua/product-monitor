using Core;
using Domain;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace Service.ProductSpecialService;

[Service(ServiceType.Scoped)]
public class ProductSpecialServiceService(Db db)
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    public void RefreshServiceCount(long collegeId, long productId)
    {
        int updateCount = db.ProductSpecialService.Count(e => e.CollegeId == collegeId && e.ProductId == productId);

        db.ProductSpecialService
            .Where(e => e.CollegeId == collegeId && e.ProductId == productId)
            .ExecuteUpdate(x =>
                x.SetProperty(e => e.ServiceCount, updateCount)
            );
    }
}