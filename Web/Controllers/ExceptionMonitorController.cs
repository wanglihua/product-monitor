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
using Web.Models.ExceptionMonitor;

namespace Web.Controllers;

[Auth(PassRes.DataMonitor.ExceptionMonitor)]
public class ExceptionMonitorController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly ActionLogService actionLogService;

    public ExceptionMonitorController(
        Db db,
        UserLoginService userLoginService,
        ActionLogService actionLogService
    )
    {
        this.db = db;
        this.userLoginService = userLoginService;
        this.actionLogService = actionLogService;
    }

    private class ExceptionMonitorJoinItem
    {
        public ExceptionMonitorEntity ExceptionMonitor { get; set; } = new();

        public ProductEntity Product { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/exception-monitor/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ExceptionMonitorListParam param)
    {
        IQueryable<ExceptionMonitorJoinItem> query = db.Set<ExceptionMonitorEntity>()
            .Join(db.Set<ProductEntity>(),
                em => em.ProductId,
                p => p.Id,
                (em, p) => new ExceptionMonitorJoinItem {
                    ExceptionMonitor = em,
                    Product = p,
                }
            );

        if (string.IsNullOrWhiteSpace(param.ProductNameQuery) == false)
        {
            string productNameQuery = param.ProductNameQuery.Trim();
            query = query.Where(e => e.Product.Name.Contains(productNameQuery));
        }

        if (param.ProvinceTypeQuery != ProvinceType.None)
        {
            var provinceTypeQuery = param.ProvinceTypeQuery;
            query = query.Where(e => e.ExceptionMonitor.ProvinceType == provinceTypeQuery);
        }

        int exceptionMonitorCount = query.Count();

        // 排序字段
        Expression<Func<ExceptionMonitorJoinItem, object>> sortField = e => e.ExceptionMonitor.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();

            if (sortFieldParam == nameof(ExceptionMonitorListItem.CreateTime).ToUpper())
            {
                sortField = e => e.ExceptionMonitor.CreateTime;
            }

            if (sortFieldParam == nameof(ExceptionMonitorListItem.UpdateTime).ToUpper())
            {
                sortField = e => e.ExceptionMonitor.UpdateTime;
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

        var exceptionMonitorList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var exceptionMonitorItemList = exceptionMonitorList
            .Select(e => new ExceptionMonitorListItem {
                Id = e.ExceptionMonitor.Id,
                ProductName = e.Product.Name,
                IpAddress = e.ExceptionMonitor.IpAddress,
                ProvinceType = e.ExceptionMonitor.ProvinceType.Name,
                Port = e.ExceptionMonitor.Port.ToString(),
                CreateTime = e.ExceptionMonitor.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.ExceptionMonitor.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["exceptionMonitorList"] = exceptionMonitorItemList,
                ["exceptionMonitorCount"] = exceptionMonitorCount,
            },
        });
    }

    [HttpPost]
    [Route("/api/exception-monitor/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        long exceptionMonitorId = param.Id;

        var model = new ExceptionMonitorDetailModel {
            Id = 0,
            ProductId = 0,
            IpAddress = "",
            ProvinceType = 0,
            Port = 0,
        };

        if (exceptionMonitorId > 0)
        {
            var exceptionMonitor = db.Set<ExceptionMonitorEntity>().Find(exceptionMonitorId);
            if (exceptionMonitor != null)
            {
                model = new ExceptionMonitorDetailModel {
                    Id = exceptionMonitor.Id,
                    ProductId = exceptionMonitor.ProductId,
                    IpAddress = exceptionMonitor.IpAddress,
                    ProvinceType = exceptionMonitor.ProvinceType.Id,
                    Port = exceptionMonitor.Port,
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
    [Route("/api/exception-monitor/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long exceptionMonitorId = param.Id;
        var exceptionMonitor = db.Set<ExceptionMonitorEntity>().Find(exceptionMonitorId);
        if (exceptionMonitor == null)
        {
            throw new ValidationException($"指定的异常检测#{exceptionMonitorId}不存在!");
        }

        db.Remove<ExceptionMonitorEntity>(exceptionMonitor);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"#{exceptionMonitor.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ExceptionMonitorDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}