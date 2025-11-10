using System.Linq.Expressions;
using Core;
using Core.Auth;
using Core.Database;
using Core.Util;
using Core.Web;
using Domain;
using Domain.PassRes;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Service;
using Web.Models.ProductUrlMonitor;

namespace Web.Controllers;

[Auth(PassRes.DataMonitor.ProductUrlMonitor)]
public class ProductUrlMonitorController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly ActionLogService actionLogService;

    public ProductUrlMonitorController(
        Db db,
        UserLoginService userLoginService,
        ActionLogService actionLogService
    )
    {
        this.db = db;
        this.userLoginService = userLoginService;
        this.actionLogService = actionLogService;
    }

    private class ProductUrlMonitorJoinItem
    {
        public ProductUrlMonitorEntity ProductUrlMonitor { get; set; } = new();

        public CollegeEntity College { get; set; } = new();

        public ProductEntity Product { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/product-url-monitor/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductUrlMonitorListParam param)
    {
        IQueryable<ProductUrlMonitorJoinItem> query = db.Set<ProductUrlMonitorEntity>()
            .Join(db.Set<CollegeEntity>(),
                pum => pum.CollegeId,
                c => c.Id,
                (pum, c) => new ProductUrlMonitorJoinItem {
                    ProductUrlMonitor = pum,
                    College = c,
                })
            .Join(db.Set<ProductEntity>(),
                pum => pum.ProductUrlMonitor.ProductId,
                p => p.Id,
                (pum, p) => new ProductUrlMonitorJoinItem {
                    ProductUrlMonitor = pum.ProductUrlMonitor,
                    College = pum.College,
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

        if (param.ProductUrlTypeQuery != ProductUrlType.None)
        {
            var productUrlTypeQuery = param.ProductUrlTypeQuery;
            query = query.Where(e => e.ProductUrlMonitor.ProductUrlType == productUrlTypeQuery);
        }

        int productUrlMonitorCount = query.Count();

        // 排序字段
        Expression<Func<ProductUrlMonitorJoinItem, object>> sortField = e => e.ProductUrlMonitor.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();

            if (sortFieldParam == nameof(ProductUrlMonitorListItem.CreateTime).ToUpper())
            {
                sortField = e => e.ProductUrlMonitor.CreateTime;
            }

            if (sortFieldParam == nameof(ProductUrlMonitorListItem.UpdateTime).ToUpper())
            {
                sortField = e => e.ProductUrlMonitor.UpdateTime;
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

        var productUrlMonitorList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var productUrlMonitorItemList = productUrlMonitorList
            .Select(e => new ProductUrlMonitorListItem {
                Id = e.ProductUrlMonitor.Id,
                CollegeName = e.College.Name,
                ProductName = e.Product.Name,
                ProductUrlType = e.ProductUrlMonitor.ProductUrlType.Name,
                TaskCount = e.ProductUrlMonitor.TaskCount,
                TeacherCount = e.ProductUrlMonitor.TeacherCount,
                CreateTime = e.ProductUrlMonitor.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.ProductUrlMonitor.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        var productUrlTypeList = new List<Dictionary<string, object>> {
            new Dictionary<string, object> {
                ["value"] = "",
                ["label"] = "分类查询",
            }
        };

        productUrlTypeList.AddRange(ProductUrlType.GetList()
            .Select(e => new Dictionary<string, object> {
                ["value"] = e.Id,
                ["label"] = e.Name,
            })
        );

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["productUrlMonitorList"] = productUrlMonitorItemList,
                ["productUrlMonitorCount"] = productUrlMonitorCount,
                ["productUrlTypeList"] = productUrlTypeList,
            },
        });
    }

    [HttpPost]
    [Route("/api/product-url-monitor/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long productUrlMonitorId = param.Id;

        var model = new ProductUrlMonitorDetailModel {
            Id = 0,
            CollegeId = 0,
            ProductId = 0,
            ProductUrlType = 0,
            TaskCount = 0,
            TeacherCount = 0,
        };

        if (productUrlMonitorId > 0)
        {
            var productUrlMonitor = db.Set<ProductUrlMonitorEntity>().Find(productUrlMonitorId);
            if (productUrlMonitor != null)
            {
                model = new ProductUrlMonitorDetailModel {
                    Id = productUrlMonitor.Id,
                    CollegeId = productUrlMonitor.CollegeId,
                    ProductId = productUrlMonitor.ProductId,
                    ProductUrlType = productUrlMonitor.ProductUrlType.Id,
                    TaskCount = productUrlMonitor.TaskCount,
                    TeacherCount = productUrlMonitor.TeacherCount,
                };
            }
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = model,
        });
    }

    [HttpPost]
    [Route("/api/product-url-monitor/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long productUrlMonitorId = param.Id;
        var productUrlMonitor = db.Set<ProductUrlMonitorEntity>().Find(productUrlMonitorId);
        if (productUrlMonitor == null)
        {
            throw new ValidationException($"指定的网站监测#{productUrlMonitorId}不存在!");
        }

        db.Remove<ProductUrlMonitorEntity>(productUrlMonitor);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"#{productUrlMonitor.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductUrlMonitorDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}