using Core;
using Core.Config;
using Domain;
using Domain.Dto;
using Microsoft.EntityFrameworkCore;

namespace Service;

[Service(ServiceType.Scoped)]
public class ProductSelectService(Db db)
{
    private List<long> GetProductIdList(ProjectType projectType, long collegeId)
    {
        if (projectType == ProjectType.Sale)
        {
            var query = db.ProductSale.AsNoTracking();

            query = query.Where(e => e.CollegeId == collegeId);

            var productIdList = query
                .OrderBy(e => e.Id)
                .Select(e => e.ProductId)
                .ToList();

            return productIdList;
        }

        if (projectType == ProjectType.Rent)
        {
            var query = db.ProductRent.AsNoTracking();

            query = query.Where(e => e.CollegeId == collegeId);

            var productIdList = query
                .OrderBy(e => e.Id)
                .Select(e => e.ProductId)
                .ToList();

            return productIdList;
        }

        if (projectType == ProjectType.Eval)
        {
            var query = db.ProductEval.AsNoTracking();

            query = query.Where(e => e.CollegeId == collegeId);

            var productIdList = query
                .OrderBy(e => e.Id)
                .Select(e => e.ProductId)
                .ToList();

            return productIdList;
        }

        return new List<long>();
    }

    public List<SelectOptionLongItem> GetProductSelectOptionList(ProjectType projectType, long collegeId)
    {
        var productIdList = GetProductIdList(projectType, collegeId);

        var productList = db.Product.AsNoTracking()
            .Where(e => productIdList.Contains(e.Id))
            .OrderBy(e => e.Id)
            .ToList();

        var productItemList = productList
            .Select(e => new SelectOptionLongItem {
                Value = e.Id,
                Label = e.Name,
            })
            .ToList();

        return productItemList;
    }
}