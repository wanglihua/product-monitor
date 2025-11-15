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
using Web.Models.ProductInstallMonitorEntry;

namespace Web.Controllers;

[Auth(PassRes.DataMonitor.ProductInstallMonitor)]
public class ProductInstallMonitorEntryController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly ActionLogService actionLogService;

    public ProductInstallMonitorEntryController(
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
    [Route("/api/product-install-monitor-entry/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductInstallMonitorEntryListParam param)
    {
        ValidationUtil.Validate<ProductInstallMonitorEntryListParamValidator, ProductInstallMonitorEntryListParam>(param);

        long productInstallMonitorId = param.ProductInstallMonitorId;
        var productInstallMonitor = db.Set<ProductInstallMonitorEntity>().Find(productInstallMonitorId);
        if (productInstallMonitor is null)
        {
            throw new ValidationException($"指定的安装检测#{productInstallMonitorId}不存在!");
        }

        long collegeId = productInstallMonitor.CollegeId;
        var college = db.Set<CollegeEntity>().Find(collegeId);
        if (college is null)
        {
            throw new ValidationException($"指定学校#{collegeId}不存在!");
        }

        string collegeName = college.Name;

        long productId = productInstallMonitor.ProductId;
        var product = db.Set<ProductEntity>().Find(productId);
        if (product is null)
        {
            throw new ValidationException($"指定产品#{productId}不存在!");
        }

        string productName = product.Name;

        IQueryable<ProductInstallMonitorEntryEntity> query = db.Set<ProductInstallMonitorEntryEntity>();

        query = query.Where(p => p.ProductMonitorId == productInstallMonitorId);

        int productInstallMonitorEntryCount = query.Count();

        // 排序字段
        Expression<Func<ProductInstallMonitorEntryEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(ProductInstallMonitorEntryListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.CreateTime;
            }

            if (string.Compare(sortFieldParam, nameof(ProductInstallMonitorEntryListItem.UpdateTime), StringComparison.OrdinalIgnoreCase) == 0)
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

        var productInstallMonitorEntryList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .Select(e => new {
                Id = e.Id,
                LoginUser = e.LoginUser,
                AccountType = e.AccountType,
                IpAddress = e.IpAddress,
                ProvinceType = e.ProvinceType,
                CreateTime = e.CreateTime,
                UpdateTime = e.UpdateTime,
            })
            .ToList();

        var productInstallMonitorEntryItemList = productInstallMonitorEntryList
            .Select(e => new ProductInstallMonitorEntryListItem {
                Id = e.Id,
                LoginUser = e.LoginUser,
                AccountType = e.AccountType.Name,
                IpAddress = e.IpAddress,
                ProvinceType = e.ProvinceType.Name,
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["productInstallMonitorEntryList"] = productInstallMonitorEntryItemList,
                ["productInstallMonitorEntryCount"] = productInstallMonitorEntryCount,
                ["collegeName"] = collegeName,
                ["productName"] = productName,
            },
        });
    }

    [HttpPost]
    [Route("/api/product-install-monitor-entry/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamEmptyValidator, IdParam>(param);

        long productInstallMonitorEntryId = param.Id;

        var model = new ProductInstallMonitorEntryDetailModel {
            Id = 0,
            ProductInstallMonitorId = 0,
            LoginUser = "",
            AccountType = 0,
            IpAddress = "",
            ProvinceType = 0,
        };

        if (productInstallMonitorEntryId > 0)
        {
            var productInstallMonitorEntry = db.Set<ProductInstallMonitorEntryEntity>().Find(productInstallMonitorEntryId);
            if (productInstallMonitorEntry is not null)
            {
                model = new ProductInstallMonitorEntryDetailModel {
                    Id = productInstallMonitorEntry.Id,
                    ProductInstallMonitorId = productInstallMonitorEntry.ProductMonitorId,
                    LoginUser = productInstallMonitorEntry.LoginUser,
                    AccountType = productInstallMonitorEntry.AccountType.Id,
                    IpAddress = productInstallMonitorEntry.IpAddress,
                    ProvinceType = productInstallMonitorEntry.ProvinceType.Id,
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
    [Route("/api/product-install-monitor-entry/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long productInstallMonitorEntryId = param.Id;
        var productInstallMonitorEntry = db.Set<ProductInstallMonitorEntryEntity>().Find(productInstallMonitorEntryId);
        if (productInstallMonitorEntry is null)
        {
            throw new ValidationException($"指定的院校使用详情#{productInstallMonitorEntryId}不存在!");
        }

        db.Remove<ProductInstallMonitorEntryEntity>(productInstallMonitorEntry);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"#{productInstallMonitorEntry.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductInstallMonitorEntryDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}