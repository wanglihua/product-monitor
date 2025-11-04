using Core;
using Domain;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class ProductService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;

    public ProductService(Db db)
    {
        this.db = db;
    }

    public void UpdateLatestVersionData(long productId)
    {
        var product = db.Set<ProductEntity>().Find(productId);
        if (product == null)
        {
            throw new ValidationException($"指定的产品#{productId}不存在!");
        }

        var versionCount = db.Set<ProductVersionEntity>()
            .Count(e => e.ProductId == productId);

        product.VersionCount = versionCount;

        var productLatestVersion = db.Set<ProductVersionEntity>().Where(e => e.ProductId == productId)
            .OrderByDescending(e => e.Id)
            .Take(1)
            .SingleOrDefault();

        if (productLatestVersion != null)
        {
            product.LatestVersion = productLatestVersion.Version;
            product.VersionUpdateTime = productLatestVersion.VersionUpdateTime;
        }
        else
        {
            product.LatestVersion = "";
            product.VersionUpdateTime = null;
        }

        db.Update<ProductEntity>(product);
        db.SaveChanges();
    }
}