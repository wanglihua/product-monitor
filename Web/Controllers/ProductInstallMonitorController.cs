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

    private class ProductInstallMonitorJoinItem
    {
        public ProductInstallMonitorEntity ProductInstallMonitor { get; set; } = new();

        public CollegeEntity College { get; set; } = new();

        public ProductEntity Product { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/product-install-monitor/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductInstallMonitorListParam param)
    {
        ValidationUtil.Validate<ProductInstallMonitorListParamValidator, ProductInstallMonitorListParam>(param);

        IQueryable<ProductInstallMonitorJoinItem> query = db.Set<ProductInstallMonitorEntity>()
            .Join(db.Set<CollegeEntity>(),
                pim => pim.CollegeId,
                c => c.Id,
                (pim, c) => new ProductInstallMonitorJoinItem {
                    ProductInstallMonitor = pim,
                    College = c,
                })
            .Join(db.Set<ProductEntity>(),
                pim => pim.ProductInstallMonitor.ProductId,
                p => p.Id,
                (pim, p) => new ProductInstallMonitorJoinItem {
                    ProductInstallMonitor = pim.ProductInstallMonitor,
                    College = pim.College,
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

        int productInstallMonitorCount = query.Count();

        // 排序字段
        Expression<Func<ProductInstallMonitorJoinItem, object>> sortField = e => e.ProductInstallMonitor.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(ProductInstallMonitorListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductInstallMonitor.CreateTime;
            }

            if (string.Compare(sortFieldParam, nameof(ProductInstallMonitorListItem.UpdateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductInstallMonitor.UpdateTime;
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
            .Select(e => new {
                Id = e.ProductInstallMonitor.Id,
                CollegeName = e.College.Name,
                ProductName = e.Product.Name,
                ProvinceType = e.ProductInstallMonitor.ProvinceType,
                CpuSerialNo = e.ProductInstallMonitor.CpuSerialNo,
                NetMacAddress = e.ProductInstallMonitor.NetMacAddress,
                TaskCount = e.ProductInstallMonitor.TaskCount,
                TeacherCount = e.ProductInstallMonitor.TeacherCount,
                CreateTime = e.ProductInstallMonitor.CreateTime,
                UpdateTime = e.ProductInstallMonitor.UpdateTime,
            })
            .ToList();

        var productInstallMonitorItemList = productInstallMonitorList
            .Select(e => new ProductInstallMonitorListItem {
                Id = e.Id,
                CollegeName = e.CollegeName,
                ProductName = e.ProductName,
                Province = e.ProvinceType.Name,
                CpuSerialNo = e.CpuSerialNo,
                NetMacAddress = e.NetMacAddress,
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
        ValidationUtil.Validate<IdParamEmptyValidator, IdParam>(param);

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
            if (productInstallMonitor is not null)
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
    [Route("/api/product-install-monitor/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long productInstallMonitorId = param.Id;
        var productInstallMonitor = db.Set<ProductInstallMonitorEntity>().Find(productInstallMonitorId);
        if (productInstallMonitor is null)
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