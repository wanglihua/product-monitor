using System.Linq.Expressions;
using Core.Auth;
using Core.Database;
using Core.Web;
using Domain;
using Domain.PassRes;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Service;
using Web.Models.Product;

namespace Web.Controllers;

[Auth(PassRes.BasicSetting.Product)]
public class ProductController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly ActionLogService actionLogService;

    public ProductController(
        Db db,
        UserLoginService userLoginService,
        ActionLogService actionLogService
    )
    {
        this.db = db;
        this.userLoginService = userLoginService;
        this.actionLogService = actionLogService;
    }

    [HttpPost]
    [Route("/api/product/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductListParam param)
    {
        IQueryable<ProductEntity> query = db.Set<ProductEntity>();

        if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        {
            string nameQuery = param.NameQuery.Trim();
            query = query.Where(p => p.Name.Contains(nameQuery));
        }

        int productCount = query.Count();

        // 排序字段
        Expression<Func<ProductEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();

            if (sortFieldParam == nameof(ProductListItem.CreateTime).ToUpper())
            {
                sortField = e => e.CreateTime;
            }

            if (sortFieldParam == nameof(ProductListItem.UpdateTime).ToUpper())
            {
                sortField = e => e.UpdateTime;
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

        var productList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var productItemList = productList
            .Select(e => new ProductListItem {
                Id = e.Id,
                Name = e.Name,
                Version = e.Version,
                VersionUpdateBase = e.VersionUpdateBase,
                VersionUpdateDesc = e.VersionUpdateDesc,
                VersionUpdateTime = e.VersionUpdateTime.ToString("yyyy-MM-dd"),
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["productList"] =  productItemList,
                ["productCount"] =  productCount,
            },
        });
    }
}