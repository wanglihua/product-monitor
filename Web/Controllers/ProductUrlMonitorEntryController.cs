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
using Web.Models.ProductUrlMonitorEntry;

namespace Web.Controllers;

[Auth(PassRes.DataMonitor.ProductUrlMonitor)]
public class ProductUrlMonitorEntryController(
    Db db,
    UserLoginService userLoginService,
    ActionLogService actionLogService) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    [HttpPost]
    [Route("/api/product-url-monitor-entry/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductUrlMonitorEntryListParam param)
    {
        ValidationUtil.Validate<ProductUrlMonitorEntryListParamValidator, ProductUrlMonitorEntryListParam>(param);

        long productUrlMonitorId = param.ProductUrlMonitorId;
        var productUrlMonitor = db.ProductUrlMonitor.Find(productUrlMonitorId);
        if (productUrlMonitor is null)
        {
            throw new ValidationException($"指定的网站监测#{productUrlMonitorId}不存在!");
        }

        long collegeId = productUrlMonitor.CollegeId;
        var college = db.College.Find(collegeId);
        if (college is null)
        {
            throw new ValidationException($"指定学校#{collegeId}不存在!");
        }

        string collegeName = college.Name;

        long productId = productUrlMonitor.ProductId;
        var product = db.Product.Find(productId);
        if (product is null)
        {
            throw new ValidationException($"指定产品#{productId}不存在!");
        }

        string productName = product.Name;

        IQueryable<ProductUrlMonitorEntryEntity> query = db.ProductUrlMonitorEntry.AsNoTracking();
        query = query.Where(e => e.ProductUrlMonitorId == productUrlMonitorId);

        int productUrlMonitorEntryCount = query.Count();

        // 排序字段
        Expression<Func<ProductUrlMonitorEntryEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (sortFieldParam == nameof(ProductUrlMonitorEntryListItem.CreateTime))
            {
                sortField = e => e.CreateTime;
            }

            if (sortFieldParam == nameof(ProductUrlMonitorEntryListItem.UpdateTime))
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

        var productUrlMonitorEntryList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .Select(e => new {
                Id = e.Id,
                ProductUrlMonitorId = e.ProductUrlMonitorId,
                LoginUser = e.LoginUser,
                LoginPassword = e.LoginPassword,
                IpAddress = e.IpAddress,
                ProvinceType = e.ProvinceType,
                CreateTime = e.CreateTime,
                UpdateTime = e.UpdateTime,
            })
            .ToList();

        var productUrlMonitorEntryItemList = productUrlMonitorEntryList
            .Select(e => new ProductUrlMonitorEntryListItem {
                Id = e.Id,
                ProductUrlMonitorId = e.ProductUrlMonitorId,
                LoginUser = e.LoginUser,
                LoginPassword = e.LoginPassword,
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
                ["productUrlMonitorEntryList"] = productUrlMonitorEntryItemList,
                ["productUrlMonitorEntryCount"] = productUrlMonitorEntryCount,
                ["collegeName"] = collegeName,
                ["productName"] = productName,
            },
        });
    }

    [HttpPost]
    [Route("/api/product-url-monitor-entry/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamEmptyValidator, IdParam>(param);

        long productUrlMonitorEntryId = param.Id;

        var model = new ProductUrlMonitorEntryDetailModel {
            Id = 0,
            ProductUrlMonitorId = 0,
            LoginUser = "",
            LoginPassword = "",
            IpAddress = "",
            ProvinceType = 0,
        };

        if (productUrlMonitorEntryId > 0)
        {
            var productUrlMonitorEntry = db.ProductUrlMonitorEntry.Find(productUrlMonitorEntryId);
            if (productUrlMonitorEntry is not null)
            {
                model = new ProductUrlMonitorEntryDetailModel {
                    Id = productUrlMonitorEntry.Id,
                    ProductUrlMonitorId = productUrlMonitorEntry.ProductUrlMonitorId,
                    LoginUser = productUrlMonitorEntry.LoginUser,
                    LoginPassword = productUrlMonitorEntry.LoginPassword,
                    IpAddress = productUrlMonitorEntry.IpAddress,
                    ProvinceType = productUrlMonitorEntry.ProvinceType.Id,
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
    [Route("/api/product-url-monitor-entry/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] ProductUrlMonitorEntryDetailModel model)
    {
        ValidationUtil.Validate<ProductUrlMonitorEntryDetailModelValidator, ProductUrlMonitorEntryDetailModel>(model);

        long productUrlMonitorEntryId = model.Id;
        bool isCreate = productUrlMonitorEntryId == 0;

        ProductUrlMonitorEntryEntity? productUrlMonitorEntry;
        if (isCreate == true)
        {
            productUrlMonitorEntry = new ProductUrlMonitorEntryEntity();
        }
        else
        {
            productUrlMonitorEntry = db.ProductUrlMonitorEntry.Find(productUrlMonitorEntryId);
            if (productUrlMonitorEntry is null)
            {
                throw new ValidationException($"指定的网站监测详情#{productUrlMonitorEntryId}不存在!");
            }
        }

        productUrlMonitorEntry.ProductUrlMonitorId = model.ProductUrlMonitorId;
        productUrlMonitorEntry.LoginUser = model.LoginUser ?? "";
        productUrlMonitorEntry.LoginPassword = model.LoginPassword ?? "";
        productUrlMonitorEntry.IpAddress = model.IpAddress ?? "";
        productUrlMonitorEntry.ProvinceType = ProvinceType.GetById(model.ProvinceType);

        if (isCreate == true)
        {
            db.ProductUrlMonitorEntry.Add(productUrlMonitorEntry);
        }

        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        if (isCreate == true)
        {
            string actionDesc = $"#{productUrlMonitorEntry.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductUrlMonitorEntryCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"#{productUrlMonitorEntry.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductUrlMonitorEntryUpdate, actionDesc);
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/product-url-monitor-entry/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long productUrlMonitorEntryId = param.Id;
        var productUrlMonitorEntry = db.ProductUrlMonitorEntry.Find(productUrlMonitorEntryId);
        if (productUrlMonitorEntry is null)
        {
            throw new ValidationException($"指定的网站监测详情#{productUrlMonitorEntryId}不存在!");
        }

        db.ProductUrlMonitorEntry.Remove(productUrlMonitorEntry);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"#{productUrlMonitorEntry.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductUrlMonitorEntryDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}