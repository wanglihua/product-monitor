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
using Web.Models.ProductInstallMonitor;

namespace Web.Controllers;

[Auth(PassRes.DataMonitor.ProductInstallMonitor)]
public class ProductInstallMonitorController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly ActionLogService actionLogService;

    public ProductInstallMonitorController(
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
    [Route("/api/product-install-monitor/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductInstallMonitorListParam param)
    {
        IQueryable<ProductInstallMonitorEntity> query = db.Set<ProductInstallMonitorEntity>();

        // if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        // {
        //     string nameQuery = param.NameQuery.Trim();
        //     query = query.Where(p => p.Name.Contains(nameQuery));
        // }

        int productInstallMonitorCount = query.Count();

        // 排序字段
        Expression<Func<ProductInstallMonitorEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();

            if (sortFieldParam == nameof(ProductInstallMonitorListItem.CreateTime).ToUpper())
            {
                sortField = e => e.CreateTime;
            }

            if (sortFieldParam == nameof(ProductInstallMonitorListItem.UpdateTime).ToUpper())
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

        var productInstallMonitorList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var productInstallMonitorItemList = productInstallMonitorList
            .Select(e => new ProductInstallMonitorListItem {
                Id = e.Id,
                CollegeId = e.CollegeId.ToString(),
                ProductId = e.ProductId.ToString(),
                ProvinceType = e.ProvinceType.Name,
                CpuSerialNo = e.CpuSerialNo,
                NetMacAddress = e.NetMacAddress,
                TaskCount = e.TaskCount.ToString(),
                TeacherCount = e.TeacherCount.ToString(),
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["productInstallMonitorList"] = productInstallMonitorItemList,
                ["productInstallMonitorCount"] = productInstallMonitorCount,
            },
        });
    }

    [HttpPost]
    [Route("/api/product-install-monitor/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        long productInstallMonitorId = param.Id;

        var model = new ProductInstallMonitorDetailModel {
            Id = 0,
            CollegeId = 0,
            ProductId = 0,
            ProvinceType = 0,
            CpuSerialNo = "",
            NetMacAddress = "",
            TaskCount = 0,
            TeacherCount = 0,
        };

        if (productInstallMonitorId > 0)
        {
            var productInstallMonitor = db.Set<ProductInstallMonitorEntity>().Find(productInstallMonitorId);
            if (productInstallMonitor != null)
            {
                model = new ProductInstallMonitorDetailModel {
                    Id = productInstallMonitor.Id,
                    CollegeId = productInstallMonitor.CollegeId,
                    ProductId = productInstallMonitor.ProductId,
                    ProvinceType = productInstallMonitor.ProvinceType?.Id ?? 0,
                    CpuSerialNo = productInstallMonitor.CpuSerialNo,
                    NetMacAddress = productInstallMonitor.NetMacAddress,
                    TaskCount = productInstallMonitor.TaskCount,
                    TeacherCount = productInstallMonitor.TeacherCount,
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
    [Route("/api/product-install-monitor/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] ProductInstallMonitorDetailModel model)
    {
        ValidationUtil.Validate<ProductInstallMonitorDetailModelValidator, ProductInstallMonitorDetailModel>(model);

        long productInstallMonitorId = model.Id;
        bool isCreate = productInstallMonitorId == 0;

        ProductInstallMonitorEntity? productInstallMonitor;
        if (isCreate == true)
        {
            productInstallMonitor = new ProductInstallMonitorEntity();
        }
        else
        {
            productInstallMonitor = db.Set<ProductInstallMonitorEntity>().Find(productInstallMonitorId);
            if (productInstallMonitor == null)
            {
                throw new ValidationException($"指定的安装监测#{productInstallMonitorId}不存在!");
            }
        }

        productInstallMonitor.CollegeId = model.CollegeId;
        productInstallMonitor.ProductId = model.ProductId;
        productInstallMonitor.ProvinceType = ProvinceType.GetById(model.ProvinceType);
        productInstallMonitor.CpuSerialNo = model.CpuSerialNo ?? "";
        productInstallMonitor.NetMacAddress = model.NetMacAddress ?? "";
        productInstallMonitor.TaskCount = model.TaskCount ?? 0;
        productInstallMonitor.TeacherCount = model.TeacherCount ?? 0;

        if (isCreate == true)
        {
            db.Add<ProductInstallMonitorEntity>(productInstallMonitor);
        }
        else
        {
            db.Update<ProductInstallMonitorEntity>(productInstallMonitor);
        }

        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        if (isCreate == true)
        {
            string actionDesc = $"#{productInstallMonitor.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductInstallMonitorCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"#{productInstallMonitor.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductInstallMonitorUpdate, actionDesc);
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/product-install-monitor/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long productInstallMonitorId = param.Id;
        var productInstallMonitor = db.Set<ProductInstallMonitorEntity>().Find(productInstallMonitorId);
        if (productInstallMonitor == null)
        {
            throw new ValidationException($"指定的安装监测#{productInstallMonitorId}不存在!");
        }

        db.Remove<ProductInstallMonitorEntity>(productInstallMonitor);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"#{productInstallMonitor.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductInstallMonitorDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}