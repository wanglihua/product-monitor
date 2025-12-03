using Core;
using Domain;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace Service.Product;

[Service(ServiceType.Scoped)]
public class ProductService(Db db)
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    public void UpdateLatestVersionData(long productId)
    {
        var product = db.Product.Find(productId);
        if (product is null)
        {
            throw new ValidationException($"指定的产品#{productId}不存在!");
        }

        var versionCount = db.ProductVersion
            .Count(e => e.ProductId == productId);

        product.VersionCount = versionCount;

        var productLatestVersion = db.ProductVersion.AsTracking()
            .Where(e => e.ProductId == productId)
            .OrderByDescending(e => e.Id)
            .Take(1)
            .SingleOrDefault();

        if (productLatestVersion is not null)
        {
            product.LatestVersion = productLatestVersion.Version;
            product.VersionUpdateTime = productLatestVersion.VersionUpdateTime;
        }
        else
        {
            product.LatestVersion = "";
            product.VersionUpdateTime = null;
        }

        db.SaveChanges();
    }
}