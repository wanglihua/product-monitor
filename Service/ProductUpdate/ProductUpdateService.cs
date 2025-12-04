using Core;
using Domain;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace Service.ProductUpdate;

[Service(ServiceType.Scoped)]
public class ProductUpdateService(Db db)
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    public int GetUpdateCount(long collegeId, long productId)
    {
        int updateCount = db.ProductUpdate.Count(e => e.CollegeId == collegeId && e.ProductId == productId);
        return updateCount;
    }

    public void RefreshUpdateCount(long collegeId, long productId)
    {
        int updateCount = db.ProductUpdate.Count(e => e.CollegeId == collegeId && e.ProductId == productId);

        db.ProductUpdate
            .Where(e => e.CollegeId == collegeId && e.ProductId == productId)
            .ExecuteUpdate(x =>
                x.SetProperty(e => e.UpdateCount, updateCount)
            );
    }
}