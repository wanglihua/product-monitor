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
using Web.Models.ProductEvalEntry;

namespace Web.Controllers;

[Auth(PassRes.ProjectManage.ProductEval)]
public class ProductEvalEntryController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly ActionLogService actionLogService;

    public ProductEvalEntryController(
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
    [Route("/api/product-eval-entry/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductEvalEntryListParam param)
    {
        IQueryable<ProductEvalEntryEntity> query = db.Set<ProductEvalEntryEntity>();

        // if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        // {
        //     string nameQuery = param.NameQuery.Trim();
        //     query = query.Where(p => p.Name.Contains(nameQuery));
        // }

        int productEvalEntryCount = query.Count();

        // 排序字段
        Expression<Func<ProductEvalEntryEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();

            if (sortFieldParam == nameof(ProductEvalEntryListItem.CreateTime).ToUpper())
            {
                sortField = e => e.CreateTime;
            }

            if (sortFieldParam == nameof(ProductEvalEntryListItem.UpdateTime).ToUpper())
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

        var productEvalEntryList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var productEvalEntryItemList = productEvalEntryList
            .Select(e => new ProductEvalEntryListItem {
                Id = e.Id,
                ProductEvalId = e.ProductEvalId,
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
                ["productEvalEntryList"] = productEvalEntryItemList,
                ["productEvalEntryCount"] = productEvalEntryCount,
            },
        });
    }

    [HttpPost]
    [Route("/api/product-eval-entry/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        long productEvalEntryId = param.Id;

        var model = new ProductEvalEntryDetailModel {
            Id = 0,
            ProductEvalId = 0,
            TotalDays = 0,
            BeginDate = "",
            EndDate = "",
            Description = "",
        };

        if (productEvalEntryId > 0)
        {
            var productEvalEntry = db.Set<ProductEvalEntryEntity>().Find(productEvalEntryId);
            if (productEvalEntry != null)
            {
                model = new ProductEvalEntryDetailModel {
                    Id = productEvalEntry.Id,
                    ProductEvalId = productEvalEntry.ProductEvalId,
                    TotalDays = productEvalEntry.TotalDays,
                    BeginDate = productEvalEntry.BeginDate.ToString("yyyy-MM-dd"),
                    EndDate = productEvalEntry.EndDate.ToString("yyyy-MM-dd"),
                    Description = productEvalEntry.Description,
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
    [Route("/api/product-eval-entry/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] ProductEvalEntryDetailModel model)
    {
        ValidationUtil.Validate<ProductEvalEntryDetailModelValidator, ProductEvalEntryDetailModel>(model);

        long productEvalEntryId = model.Id;
        bool isCreate = productEvalEntryId == 0;

        ProductEvalEntryEntity? productEvalEntry;
        if (isCreate == true)
        {
            productEvalEntry = new ProductEvalEntryEntity();
        }
        else
        {
            productEvalEntry = db.Set<ProductEvalEntryEntity>().Find(productEvalEntryId);
            if (productEvalEntry == null)
            {
                throw new ValidationException($"指定的试用目录明细#{productEvalEntryId}不存在!");
            }
        }

        productEvalEntry.ProductEvalId = model.ProductEvalId;
        productEvalEntry.TotalDays = model.TotalDays;
        productEvalEntry.BeginDate = DateTime.Parse(model.BeginDate).Date;
        productEvalEntry.EndDate = DateTime.Parse(model.EndDate).Date;
        productEvalEntry.Description = model.Description ?? "";

        if (isCreate == true)
        {
            db.Add<ProductEvalEntryEntity>(productEvalEntry);
        }
        else
        {
            db.Update<ProductEvalEntryEntity>(productEvalEntry);
        }

        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        if (isCreate == true)
        {
            string actionDesc = $"#{productEvalEntry.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductEvalEntryCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"#{productEvalEntry.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductEvalEntryUpdate, actionDesc);
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/product-eval-entry/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long productEvalEntryId = param.Id;
        var productEvalEntry = db.Set<ProductEvalEntryEntity>().Find(productEvalEntryId);
        if (productEvalEntry == null)
        {
            throw new ValidationException($"指定的试用目录明细#{productEvalEntryId}不存在!");
        }

        db.Remove<ProductEvalEntryEntity>(productEvalEntry);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"#{productEvalEntry.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductEvalEntryDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}
