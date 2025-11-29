using System.Linq.Expressions;
using Core;
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
using Web.Models.ExceptionMonitor;

namespace Web.Controllers;

[Auth(PassRes.DataMonitor.ExceptionMonitor)]
public class ExceptionMonitorController(
    Db db,
    UserLoginService userLoginService,
    ActionLogService actionLogService) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

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
        ValidationUtil.Validate<ExceptionMonitorListParamValidator, ExceptionMonitorListParam>(param);

        IQueryable<ExceptionMonitorJoinItem> query = db.ExceptionMonitor.AsNoTracking()
            .Join(db.Product,
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
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(ExceptionMonitorListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ExceptionMonitor.CreateTime;
            }

            if (string.Compare(sortFieldParam, nameof(ExceptionMonitorListItem.UpdateTime), StringComparison.OrdinalIgnoreCase) == 0)
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
            .Select(e => new {
                Id = e.ExceptionMonitor.Id,
                ProductName = e.Product.Name,
                IpAddress = e.ExceptionMonitor.IpAddress,
                ProvinceType = e.ExceptionMonitor.ProvinceType,
                Port = e.ExceptionMonitor.Port,
                CreateTime = e.ExceptionMonitor.CreateTime,
                UpdateTime = e.ExceptionMonitor.UpdateTime,
            })
            .ToList();

        var exceptionMonitorItemList = exceptionMonitorList
            .Select(e => new ExceptionMonitorListItem {
                Id = e.Id,
                ProductName = e.ProductName,
                IpAddress = e.IpAddress,
                Province = e.ProvinceType.Name,
                Port = e.Port.ToString(),
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        var provinceTypeList = new List<Dictionary<string, object>> {
            new Dictionary<string, object> {
                ["value"] = "",
                ["label"] = "省份查询",
            }
        };

        provinceTypeList.AddRange(ProvinceType.GetList()
            .Select(e => new Dictionary<string, object> {
                ["value"] = e.Id,
                ["label"] = e.Name,
            })
        );

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["exceptionMonitorList"] = exceptionMonitorItemList,
                ["exceptionMonitorCount"] = exceptionMonitorCount,
                ["provinceTypeList"] = provinceTypeList,
            },
        });
    }

    [HttpPost]
    [Route("/api/exception-monitor/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamEmptyValidator, IdParam>(param);

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
            var exceptionMonitor = db.ExceptionMonitor.Find(exceptionMonitorId);
            if (exceptionMonitor is not null)
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
        var exceptionMonitor = db.ExceptionMonitor.Find(exceptionMonitorId);
        if (exceptionMonitor is null)
        {
            throw new ValidationException($"指定的异常检测#{exceptionMonitorId}不存在!");
        }

        db.ExceptionMonitor.Remove(exceptionMonitor);
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