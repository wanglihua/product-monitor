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
using Web.Models.ProductSale;

namespace Web.Controllers;

[Auth(PassRes.ProjectManage.ProductSale)]
public class ProductSaleController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly ActionLogService actionLogService;

    public ProductSaleController(
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
    [Route("/api/product-sale/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductSaleListParam param)
    {
        IQueryable<ProductSaleEntity> query = db.Set<ProductSaleEntity>();

        // if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        // {
        //     string nameQuery = param.NameQuery.Trim();
        //     query = query.Where(p => p.Name.Contains(nameQuery));
        // }

        int productSaleCount = query.Count();

        // 排序字段
        Expression<Func<ProductSaleEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();

            if (sortFieldParam == nameof(ProductSaleListItem.CreateTime).ToUpper())
            {
                sortField = e => e.CreateTime;
            }

            if (sortFieldParam == nameof(ProductSaleListItem.UpdateTime).ToUpper())
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

        var productSaleList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var productSaleItemList = productSaleList
            .Select(e => new ProductSaleListItem {
                Id = e.Id,
                CollegeId = e.CollegeId,
                ProductId = e.ProductId,
                SaleTimes = e.SaleTimes,
                InstallTimes = e.InstallTimes,
                UpdateTimes = e.UpdateTimes,
                ServiceTimes = e.ServiceTimes,
                Version = e.Version,
                SaleDate = e.SaleDate.ToString("yyyy-MM-dd"),
                ServiceExpireDate = e.ServiceExpireDate.ToString("yyyy-MM-dd"),
                Description = e.Description,
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["productSaleList"] = productSaleItemList,
                ["productSaleCount"] = productSaleCount,
            },
        });
    }

    [HttpPost]
    [Route("/api/product-sale/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        long productSaleId = param.Id;

        var model = new ProductSaleDetailModel {
            Id = 0,
            CollegeId = 0,
            ProductId = 0,
            SaleTimes = 0,
            InstallTimes = 0,
            UpdateTimes = 0,
            ServiceTimes = 0,
            Version = "",
            SaleDate = "",
            ServiceExpireDate = "",
            Description = "",
        };

        if (productSaleId > 0)
        {
            var productSale = db.Set<ProductSaleEntity>().Find(productSaleId);
            if (productSale != null)
            {
                model = new ProductSaleDetailModel {
                    Id = productSale.Id,
                CollegeId = productSale.CollegeId,
                ProductId = productSale.ProductId,
                SaleTimes = productSale.SaleTimes,
                InstallTimes = productSale.InstallTimes,
                UpdateTimes = productSale.UpdateTimes,
                ServiceTimes = productSale.ServiceTimes,
                Version = productSale.Version,
                SaleDate = productSale.SaleDate.ToString("yyyy-MM-dd"),
                ServiceExpireDate = productSale.ServiceExpireDate.ToString("yyyy-MM-dd"),
                Description = productSale.Description,
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
    [Route("/api/product-sale/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] ProductSaleDetailModel model)
    {
        ValidationUtil.Validate<ProductSaleDetailModelValidator, ProductSaleDetailModel>(model);

        long productSaleId = model.Id;
        bool isCreate = productSaleId == 0;

        ProductSaleEntity? productSale;
        if (isCreate == true)
        {
            productSale = new ProductSaleEntity();
        }
        else
        {
            productSale = db.Set<ProductSaleEntity>().Find(productSaleId);
            if (productSale == null)
            {
                throw new ValidationException($"指定的销售目录#{productSaleId}不存在!");
            }
        }

        productSale.CollegeId = model.CollegeId;
        productSale.ProductId = model.ProductId;
        productSale.SaleTimes = model.SaleTimes;
        productSale.InstallTimes = model.InstallTimes;
        productSale.UpdateTimes = model.UpdateTimes;
        productSale.ServiceTimes = model.ServiceTimes;
        productSale.Version = model.Version ?? "";
        productSale.SaleDate = DateTime.Parse(model.SaleDate).Date;
        productSale.ServiceExpireDate = DateTime.Parse(model.ServiceExpireDate).Date;
        productSale.Description = model.Description ?? "";

        if (isCreate == true)
        {
            db.Add<ProductSaleEntity>(productSale);
        }
        else
        {
            db.Update<ProductSaleEntity>(productSale);
        }

        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        if (isCreate == true)
        {
            string actionDesc = $"#{productSale.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductSaleCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"#{productSale.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductSaleUpdate, actionDesc);
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/product-sale/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long productSaleId = param.Id;
        var productSale = db.Set<ProductSaleEntity>().Find(productSaleId);
        if (productSale == null)
        {
            throw new ValidationException($"指定的销售目录#{productSaleId}不存在!");
        }

        db.Remove<ProductSaleEntity>(productSale);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"#{productSale.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductSaleDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}
