using System.Linq.Expressions;
using Core.Auth;
using Core.Database;
using Core.Util;
using Core.Web;
using Domain;
using Domain.PassRes;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Web.Models.ProductEval;

namespace Web.Controllers;

[Auth(PassRes.ProjectManage.ProductEval)]
public class ProductEvalController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;

    public ProductEvalController(
        Db db
    )
    {
        this.db = db;
    }

    private class ProductEvalJoinItem
    {
        public ProductEvalEntity ProductEval { get; set; } = new();

        public CollegeEntity College { get; set; } = new();

        public ProductEntity Product { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/product-eval/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductEvalListParam param)
    {
        ValidationUtil.Validate<ProductEvalListParamValidator, ProductEvalListParam>(param);

        IQueryable<ProductEvalJoinItem> query = db.Set<ProductEvalEntity>()
            .Join(db.Set<CollegeEntity>(),
                ps => ps.CollegeId,
                c => c.Id,
                (ps, c) => new ProductEvalJoinItem {
                    ProductEval = ps,
                    College = c,
                })
            .Join(db.Set<ProductEntity>(),
                ps => ps.ProductEval.ProductId,
                p => p.Id,
                (ps, p) => new ProductEvalJoinItem {
                    ProductEval = ps.ProductEval,
                    College = ps.College,
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

        int productEvalCount = query.Count();

        // 排序字段
        Expression<Func<ProductEvalJoinItem, object>> sortField = e => e.ProductEval.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();

            if (sortFieldParam == nameof(ProductEvalListItem.CreateTime).ToUpper())
            {
                sortField = e => e.ProductEval.CreateTime;
            }

            if (sortFieldParam == nameof(ProductEvalListItem.UpdateTime).ToUpper())
            {
                sortField = e => e.ProductEval.UpdateTime;
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

        var productEvalList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var productEvalItemList = productEvalList
            .Select(e => new ProductEvalListItem {
                Id = e.ProductEval.Id,
                CollegeName = e.College.Name,
                ProductName = e.Product.Name,
                EvalTimes = e.ProductEval.EvalTimes,
                ServiceTimes = e.ProductEval.ServiceTimes,
                TotalDays = e.ProductEval.TotalDays,
                RemainDays = e.ProductEval.RemainDays,
                BeginDate = e.ProductEval.BeginDate.ToString("yyyy-MM-dd"),
                EndDate = e.ProductEval.EndDate.ToString("yyyy-MM-dd"),
                Description = e.ProductEval.Description,
                CreateTime = e.ProductEval.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.ProductEval.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["productEvalList"] = productEvalItemList,
                ["productEvalCount"] = productEvalCount,
            },
        });
    }
}