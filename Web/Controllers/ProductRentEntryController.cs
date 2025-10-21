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
using Web.Models.ProductRentEntry;

namespace Web.Controllers;

[Auth(PassRes.ProjectManage.ProductRent)]
public class ProductRentEntryController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly ActionLogService actionLogService;

    public ProductRentEntryController(
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
    [Route("/api/product-rent-entry/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductRentEntryListParam param)
    {
        IQueryable<ProductRentEntryEntity> query = db.Set<ProductRentEntryEntity>();

        // if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        // {
        //     string nameQuery = param.NameQuery.Trim();
        //     query = query.Where(p => p.Name.Contains(nameQuery));
        // }

        int productRentEntryCount = query.Count();

        // 排序字段
        Expression<Func<ProductRentEntryEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();

            if (sortFieldParam == nameof(ProductRentEntryListItem.CreateTime).ToUpper())
            {
                sortField = e => e.CreateTime;
            }

            if (sortFieldParam == nameof(ProductRentEntryListItem.UpdateTime).ToUpper())
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

        var productRentEntryList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var productRentEntryItemList = productRentEntryList
            .Select(e => new ProductRentEntryListItem {
                Id = e.Id,
                ProductRentId = e.ProductRentId,
                TotalDays = e.TotalDays,
                BeginDate = e.BeginDate.ToString("yyyy-MM-dd"),
                EndDate = e.EndDate.ToString("yyyy-MM-dd"),
                Description = e.Description,
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["productRentEntryList"] = productRentEntryItemList,
                ["productRentEntryCount"] = productRentEntryCount,
            },
        });
    }

    [HttpPost]
    [Route("/api/product-rent-entry/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        long productRentEntryId = param.Id;

        var model = new ProductRentEntryDetailModel {
            Id = 0,
            ProductRentId = 0,
            TotalDays = 0,
            BeginDate = "",
            EndDate = "",
            Description = "",
        };

        if (productRentEntryId > 0)
        {
            var productRentEntry = db.Set<ProductRentEntryEntity>().Find(productRentEntryId);
            if (productRentEntry != null)
            {
                model = new ProductRentEntryDetailModel {
                    Id = productRentEntry.Id,
                    ProductRentId = productRentEntry.ProductRentId,
                    TotalDays = productRentEntry.TotalDays,
                    BeginDate = productRentEntry.BeginDate.ToString("yyyy-MM-dd"),
                    EndDate = productRentEntry.EndDate.ToString("yyyy-MM-dd"),
                    Description = productRentEntry.Description,
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
    [Route("/api/product-rent-entry/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] ProductRentEntryDetailModel model)
    {
        ValidationUtil.Validate<ProductRentEntryDetailModelValidator, ProductRentEntryDetailModel>(model);

        long productRentEntryId = model.Id;
        bool isCreate = productRentEntryId == 0;

        ProductRentEntryEntity? productRentEntry;
        if (isCreate == true)
        {
            productRentEntry = new ProductRentEntryEntity();
        }
        else
        {
            productRentEntry = db.Set<ProductRentEntryEntity>().Find(productRentEntryId);
            if (productRentEntry == null)
            {
                throw new ValidationException($"指定的租赁目录明细#{productRentEntryId}不存在!");
            }
        }

        productRentEntry.ProductRentId = model.ProductRentId;
        productRentEntry.TotalDays = model.TotalDays;
        productRentEntry.BeginDate = DateTime.Parse(model.BeginDate).Date;
        productRentEntry.EndDate = DateTime.Parse(model.EndDate).Date;
        productRentEntry.Description = model.Description ?? "";

        if (isCreate == true)
        {
            db.Add<ProductRentEntryEntity>(productRentEntry);
        }
        else
        {
            db.Update<ProductRentEntryEntity>(productRentEntry);
        }

        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        if (isCreate == true)
        {
            string actionDesc = $"#{productRentEntry.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductRentEntryCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"#{productRentEntry.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductRentEntryUpdate, actionDesc);
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/product-rent-entry/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long productRentEntryId = param.Id;
        var productRentEntry = db.Set<ProductRentEntryEntity>().Find(productRentEntryId);
        if (productRentEntry == null)
        {
            throw new ValidationException($"指定的租赁目录明细#{productRentEntryId}不存在!");
        }

        db.Remove<ProductRentEntryEntity>(productRentEntry);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"#{productRentEntry.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductRentEntryDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}
