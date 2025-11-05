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

    [HttpPost]
    [Route("/api/product-url-monitor/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductUrlMonitorListParam param)
    {
        IQueryable<ProductUrlMonitorEntity> query = db.Set<ProductUrlMonitorEntity>();

        // if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        // {
        //     string nameQuery = param.NameQuery.Trim();
        //     query = query.Where(p => p.Name.Contains(nameQuery));
        // }

        int productUrlMonitorCount = query.Count();

        // 排序字段
        Expression<Func<ProductUrlMonitorEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();

            if (sortFieldParam == nameof(ProductUrlMonitorListItem.CreateTime).ToUpper())
            {
                sortField = e => e.CreateTime;
            }

            if (sortFieldParam == nameof(ProductUrlMonitorListItem.UpdateTime).ToUpper())
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

        var productUrlMonitorList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var productUrlMonitorItemList = productUrlMonitorList
            .Select(e => new ProductUrlMonitorListItem {
                Id = e.Id,
                CollegeId = e.CollegeId,
                ProductId = e.ProductId,
                ProductUrlType = e.ProductUrlType.Name,
                TaskCount = e.TaskCount,
                TeacherCount = e.TeacherCount,
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["productUrlMonitorList"] = productUrlMonitorItemList,
                ["productUrlMonitorCount"] = productUrlMonitorCount,
            },
        });
    }

    [HttpPost]
    [Route("/api/product-url-monitor/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
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
    [Route("/api/product-url-monitor/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] ProductUrlMonitorDetailModel model)
    {
        ValidationUtil.Validate<ProductUrlMonitorDetailModelValidator, ProductUrlMonitorDetailModel>(model);

        long productUrlMonitorId = model.Id;
        bool isCreate = productUrlMonitorId == 0;

        ProductUrlMonitorEntity? productUrlMonitor;
        if (isCreate == true)
        {
            productUrlMonitor = new ProductUrlMonitorEntity();
        }
        else
        {
            productUrlMonitor = db.Set<ProductUrlMonitorEntity>().Find(productUrlMonitorId);
            if (productUrlMonitor == null)
            {
                throw new ValidationException($"指定的网站监测#{productUrlMonitorId}不存在!");
            }
        }

        productUrlMonitor.CollegeId = model.CollegeId;
        productUrlMonitor.ProductId = model.ProductId;
        productUrlMonitor.ProductUrlType = ProductUrlType.GetById(model.ProductUrlType);
        productUrlMonitor.TaskCount = model.TaskCount;
        productUrlMonitor.TeacherCount = model.TeacherCount;

        if (isCreate == true)
        {
            db.Add<ProductUrlMonitorEntity>(productUrlMonitor);
        }
        else
        {
            db.Update<ProductUrlMonitorEntity>(productUrlMonitor);
        }

        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        if (isCreate == true)
        {
            string actionDesc = $"#{productUrlMonitor.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductUrlMonitorCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"#{productUrlMonitor.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductUrlMonitorUpdate, actionDesc);
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
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