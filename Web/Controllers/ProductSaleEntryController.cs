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
using Web.Models.ProductSaleEntry;

namespace Web.Controllers;

[Auth(PassRes.ProjectManage.ProductSale)]
public class ProductSaleEntryController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly ActionLogService actionLogService;

    public ProductSaleEntryController(
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
    [Route("/api/product-sale-entry/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductSaleEntryListParam param)
    {
        IQueryable<ProductSaleEntryEntity> query = db.Set<ProductSaleEntryEntity>();

        // if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        // {
        //     string nameQuery = param.NameQuery.Trim();
        //     query = query.Where(p => p.Name.Contains(nameQuery));
        // }

        int productSaleEntryCount = query.Count();

        // 排序字段
        Expression<Func<ProductSaleEntryEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();

            if (sortFieldParam == nameof(ProductSaleEntryListItem.CreateTime).ToUpper())
            {
                sortField = e => e.CreateTime;
            }

            if (sortFieldParam == nameof(ProductSaleEntryListItem.UpdateTime).ToUpper())
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

        var productSaleEntryList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var productSaleEntryItemList = productSaleEntryList
            .Select(e => new ProductSaleEntryListItem {
                Id = e.Id,
                ProductSaleId = e.ProductSaleId,
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
                ["productSaleEntryList"] = productSaleEntryItemList,
                ["productSaleEntryCount"] = productSaleEntryCount,
            },
        });
    }

    [HttpPost]
    [Route("/api/product-sale-entry/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        long productSaleEntryId = param.Id;

        var model = new ProductSaleEntryDetailModel {
            Id = 0,
            ProductSaleId = 0,
            Version = "",
            SaleDate = "",
            ServiceExpireDate = "",
            Description = "",
        };

        if (productSaleEntryId > 0)
        {
            var productSaleEntry = db.Set<ProductSaleEntryEntity>().Find(productSaleEntryId);
            if (productSaleEntry != null)
            {
                model = new ProductSaleEntryDetailModel {
                    Id = productSaleEntry.Id,
                    ProductSaleId = productSaleEntry.ProductSaleId,
                    Version = productSaleEntry.Version,
                    SaleDate = productSaleEntry.SaleDate.ToString("yyyy-MM-dd"),
                    ServiceExpireDate = productSaleEntry.ServiceExpireDate.ToString("yyyy-MM-dd"),
                    Description = productSaleEntry.Description,
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
    [Route("/api/product-sale-entry/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] ProductSaleEntryDetailModel model)
    {
        ValidationUtil.Validate<ProductSaleEntryDetailModelValidator, ProductSaleEntryDetailModel>(model);

        long productSaleEntryId = model.Id;
        bool isCreate = productSaleEntryId == 0;

        ProductSaleEntryEntity? productSaleEntry;
        if (isCreate == true)
        {
            productSaleEntry = new ProductSaleEntryEntity();
        }
        else
        {
            productSaleEntry = db.Set<ProductSaleEntryEntity>().Find(productSaleEntryId);
            if (productSaleEntry == null)
            {
                throw new ValidationException($"指定的销售目录明细#{productSaleEntryId}不存在!");
            }
        }

        productSaleEntry.ProductSaleId = model.ProductSaleId;
        productSaleEntry.Version = model.Version ?? "";
        productSaleEntry.SaleDate = DateTime.Parse(model.SaleDate).Date;
        productSaleEntry.ServiceExpireDate = DateTime.Parse(model.ServiceExpireDate).Date;
        productSaleEntry.Description = model.Description ?? "";

        if (isCreate == true)
        {
            db.Add<ProductSaleEntryEntity>(productSaleEntry);
        }
        else
        {
            db.Update<ProductSaleEntryEntity>(productSaleEntry);
        }

        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        if (isCreate == true)
        {
            string actionDesc = $"#{productSaleEntry.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductSaleEntryCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"#{productSaleEntry.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductSaleEntryUpdate, actionDesc);
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/product-sale-entry/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long productSaleEntryId = param.Id;
        var productSaleEntry = db.Set<ProductSaleEntryEntity>().Find(productSaleEntryId);
        if (productSaleEntry == null)
        {
            throw new ValidationException($"指定的销售目录明细#{productSaleEntryId}不存在!");
        }

        db.Remove<ProductSaleEntryEntity>(productSaleEntry);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"#{productSaleEntry.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductSaleEntryDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}