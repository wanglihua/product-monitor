using Core;
using Domain;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace Service.ProductService;

[Service(ServiceType.Scoped)]
public class ProductServiceService(Db db)
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    public void RefreshServiceCount(long collegeId, long productId)
    {
        int updateCount = db.ProductService.Count(e => e.CollegeId == collegeId && e.ProductId == productId);

        db.ProductService
            .Where(e => e.CollegeId == collegeId && e.ProductId == productId)
            .ExecuteUpdate(x =>
                x.SetProperty(e => e.ServiceCount, updateCount)
            );
    }
}