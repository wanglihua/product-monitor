using System.Linq.Expressions;
using Core.Auth;
using Core.Database;
using Core.Util;
using Core.Web;
using Domain;
using Domain.PassRes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using Service;
using Web.Models.ProductSale;

namespace Web.Controllers;

[Auth(PassRes.ProjectManage.ProductSale)]
public class ProductSaleController(
    Db db,
    UserLoginService userLoginService,
    UserProvinceService userProvinceService) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private class ProductSaleJoinItem
    {
        public ProductSaleEntity ProductSale { get; set; } = new();

        public CollegeEntity College { get; set; } = new();

        public ProductEntity Product { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/product-sale/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductSaleListParam param)
    {
        ValidationUtil.Validate<ProductSaleListParamValidator, ProductSaleListParam>(param);

        long loginUserId = userLoginService.GetLoginUserId();
        bool isAdminLogin = userLoginService.IsAdmin(loginUserId);

        IQueryable<ProductSaleJoinItem> query = db.ProductSale
            .Join(db.College,
                ps => ps.CollegeId,
                c => c.Id,
                (ps, c) => new ProductSaleJoinItem {
                    ProductSale = ps,
                    College = c,
                })
            .Join(db.Product,
                ps => ps.ProductSale.ProductId,
                p => p.Id,
                (ps, p) => new ProductSaleJoinItem {
                    ProductSale = ps.ProductSale,
                    College = ps.College,
                    Product = p,
                })
            .AsNoTracking();

        if (isAdminLogin == false)
        {
            var userProvinceList = userProvinceService.GetUserProvinceList(loginUserId);
            query = query.Where(e => userProvinceList.Contains(e.College.Province));
        }

        if (string.IsNullOrWhiteSpace(param.CollegeNameQuery) == false)
        {
            string collegeNameQuery = param.CollegeNameQuery.Trim();
            query = query.Where(e => e.College.Name.Contains(collegeNameQuery));
        }

        if (string.IsNullOrWhiteSpace(param.ProductNameQuery) == false)
        {
            string productNameQuery = param.ProductNameQuery.Trim();
            query = query.Where(e => e.Product.Name.Contains(productNameQuery));
        }

        int productSaleCount = query.Count();

        // 排序字段
        Expression<Func<ProductSaleJoinItem, object>> sortField = e => e.ProductSale.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(ProductSaleListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductSale.CreateTime;
            }

            if (string.Compare(sortFieldParam, nameof(ProductSaleListItem.UpdateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductSale.UpdateTime;
            }
        }

        // 排序顺序
        string sortOrder = param.SortOrder;
        if (sortOrder == SortOrder.Ascend)
        {
            query = query.OrderBy(sortField);
        }
        else
        {
            query = query.OrderByDescending(sortField);
        }

        var productSaleList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .Select(e => new {
                Id = e.ProductSale.Id,
                CollegeName = e.College.Name,
                ProductName = e.Product.Name,
                SaleTimes = e.ProductSale.SaleTimes,
                InstallTimes = e.ProductSale.InstallTimes,
                UpdateTimes = e.ProductSale.UpdateTimes,
                ServiceTimes = e.ProductSale.ServiceTimes,
                Version = e.ProductSale.Version,
                SaleDate = e.ProductSale.SaleDate,
                ServiceExpireDate = e.ProductSale.ServiceExpireDate,
                Description = e.ProductSale.Description,
                CreateTime = e.ProductSale.CreateTime,
                UpdateTime = e.ProductSale.UpdateTime,
            })
            .ToList();

        var productSaleItemList = productSaleList
            .Select(e => new ProductSaleListItem {
                Id = e.Id,
                CollegeName = e.CollegeName,
                ProductName = e.ProductName,
                SaleTimes = e.SaleTimes,
                InstallTimes = e.InstallTimes,
                UpdateTimes = e.UpdateTimes,
                ServiceTimes = e.ServiceTimes,
                Version = e.Version,
                SaleDate = e.SaleDate.ToString("yyyy-MM-dd"),
                ServiceExpireDate = e.ServiceExpireDate.ToString("yyyy-MM-dd"),
                Description = e.Description,
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["productSaleList"] = productSaleItemList,
                ["productSaleCount"] = productSaleCount,
            },
        });
    }
}