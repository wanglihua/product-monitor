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

    [HttpPost]
    [Route("/api/exception-monitor/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ExceptionMonitorListParam param)
    {
        IQueryable<ExceptionMonitorEntity> query = db.Set<ExceptionMonitorEntity>();

        // if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        // {
        //     string nameQuery = param.NameQuery.Trim();
        //     query = query.Where(p => p.Name.Contains(nameQuery));
        // }

        int exceptionMonitorCount = query.Count();

        // 排序字段
        Expression<Func<ExceptionMonitorEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();

            if (sortFieldParam == nameof(ExceptionMonitorListItem.CreateTime).ToUpper())
            {
                sortField = e => e.CreateTime;
            }

            if (sortFieldParam == nameof(ExceptionMonitorListItem.UpdateTime).ToUpper())
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

        var exceptionMonitorList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var exceptionMonitorItemList = exceptionMonitorList
            .Select(e => new ExceptionMonitorListItem {
                Id = e.Id,
                ProductId = e.ProductId,
                IpAddress = e.IpAddress,
                ProvinceType = e.ProvinceType.Name,
                Port = e.Port.ToString(),
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
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
    [Route("/api/exception-monitor/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] ExceptionMonitorDetailModel model)
    {
        ValidationUtil.Validate<ExceptionMonitorDetailModelValidator, ExceptionMonitorDetailModel>(model);

        long exceptionMonitorId = model.Id;
        bool isCreate = exceptionMonitorId == 0;

        ExceptionMonitorEntity? exceptionMonitor;
        if (isCreate == true)
        {
            exceptionMonitor = new ExceptionMonitorEntity();
        }
        else
        {
            exceptionMonitor = db.Set<ExceptionMonitorEntity>().Find(exceptionMonitorId);
            if (exceptionMonitor == null)
            {
                throw new ValidationException($"指定的异常检测#{exceptionMonitorId}不存在!");
            }
        }

        exceptionMonitor.ProductId = model.ProductId;
        exceptionMonitor.IpAddress = model.IpAddress ?? "";
        exceptionMonitor.ProvinceType = ProvinceType.GetById(model.ProvinceType);
        exceptionMonitor.Port = model.Port;

        if (isCreate == true)
        {
            db.Add<ExceptionMonitorEntity>(exceptionMonitor);
        }
        else
        {
            db.Update<ExceptionMonitorEntity>(exceptionMonitor);
        }

        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        if (isCreate == true)
        {
            string actionDesc = $"#{exceptionMonitor.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ExceptionMonitorCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"#{exceptionMonitor.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ExceptionMonitorUpdate, actionDesc);
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
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