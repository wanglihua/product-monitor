using System.Linq.Expressions;
using Core.Auth;
using Core.Database;
using Core.Util;
using Core.Web;
using Domain;
using Domain.PassRes;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Web.Models.ProductRent;

namespace Web.Controllers;

[Auth(PassRes.ProjectManage.ProductRent)]
public class ProductRentController(Db db) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private class ProductRentJoinItem
    {
        public ProductRentEntity ProductRent { get; set; } = new();

        public CollegeEntity College { get; set; } = new();

        public ProductEntity Product { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/product-rent/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductRentListParam param)
    {
        ValidationUtil.Validate<ProductRentListParamValidator, ProductRentListParam>(param);

        IQueryable<ProductRentJoinItem> query = db.ProductRent
            .Join(db.College,
                pr => pr.CollegeId,
                c => c.Id,
                (pr, c) => new ProductRentJoinItem {
                    ProductRent = pr,
                    College = c,
                })
            .Join(db.Product,
                pr => pr.ProductRent.ProductId,
                p => p.Id,
                (pr, p) => new ProductRentJoinItem {
                    ProductRent = pr.ProductRent,
                    College = pr.College,
                    Product = p,
                });

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

        int productRentCount = query.Count();

        // 排序字段
        Expression<Func<ProductRentJoinItem, object>> sortField = e => e.ProductRent.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(ProductRentListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductRent.CreateTime;
            }

            if (string.Compare(sortFieldParam, nameof(ProductRentListItem.UpdateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductRent.UpdateTime;
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

        var productRentList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .Select(e => new {
                Id = e.ProductRent.Id,
                CollegeName = e.College.Name,
                ProductName = e.Product.Name,
                RentTimes = e.ProductRent.RentTimes,
                ServiceTimes = e.ProductRent.ServiceTimes,
                TotalDays = e.ProductRent.TotalDays,
                RemainDays = e.ProductRent.RemainDays,
                BeginDate = e.ProductRent.BeginDate,
                EndDate = e.ProductRent.EndDate,
                Description = e.ProductRent.Description,
                CreateTime = e.ProductRent.CreateTime,
                UpdateTime = e.ProductRent.UpdateTime,
            })
            .ToList();

        var productRentItemList = productRentList
            .Select(e => new ProductRentListItem {
                Id = e.Id,
                CollegeName = e.CollegeName,
                ProductName = e.ProductName,
                RentTimes = e.RentTimes,
                ServiceTimes = e.ServiceTimes,
                TotalDays = e.TotalDays,
                RemainDays = e.RemainDays,
                BeginDate = e.BeginDate.ToString("yyyy-MM-dd"),
                EndDate = e.EndDate.ToString("yyyy-MM-dd"),
                Description = e.Description,
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["productRentList"] = productRentItemList,
                ["productRentCount"] = productRentCount,
            },
        });
    }
}