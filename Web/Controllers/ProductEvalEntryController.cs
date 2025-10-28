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
    private readonly ProductEvalService productEvalService;

    public ProductEvalEntryController(
        Db db,
        UserLoginService userLoginService,
        ActionLogService actionLogService,
        ProductEvalService productEvalService
    )
    {
        this.db = db;
        this.userLoginService = userLoginService;
        this.actionLogService = actionLogService;
        this.productEvalService = productEvalService;
    }

    [HttpPost]
    [Route("/api/product-eval-entry/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductEvalEntryListParam param)
    {
        ValidationUtil.Validate<ProductEvalEntryListParamValidator, ProductEvalEntryListParam>(param);

        IQueryable<ProductEvalEntryEntity> query = db.Set<ProductEvalEntryEntity>();

        long productEvalId = param.ProductEvalId;
        var productEval = db.Set<ProductEvalEntity>().Find(productEvalId);
        if (productEval == null)
        {
            throw new ValidationException($"指定的试用目录#{productEvalId}不存在!");
        }

        long collegeId = productEval.CollegeId;
        var college = db.Set<CollegeEntity>().Find(collegeId);
        if (college == null)
        {
            throw new ValidationException($"指定学校#{collegeId}不存在!");
        }

        string collegeName = college.Name;

        long productId = productEval.ProductId;
        var product = db.Set<ProductEntity>().Find(productId);
        if (product == null)
        {
            throw new ValidationException($"指定产品#{productId}不存在!");
        }

        string productName = product.Name;

        query = query.Where(p => p.ProductEvalId == productEvalId);

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
                ["collegeName"] = collegeName,
                ["productName"] = productName,
            },
        });
    }

    [HttpPost]
    [Route("/api/product-eval-entry/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] ProductEvalEntryDetailParam param)
    {
        long productEvalEntryId = param.ProductEvalEntryId;

        long collegeId = 0;
        string collegeName = "";
        long productId = 0;
        string productName = "";

        long productEvalId = param.ProductEvalId;
        if (productEvalEntryId > 0)
        {
            var productEvalEntry = db.Set<ProductEvalEntryEntity>().Find(productEvalEntryId);
            if (productEvalEntry == null)
            {
                throw new ValidationException($"指定的试用目录明细#{productEvalEntryId}不存在!");
            }

            productEvalId = productEvalEntry.ProductEvalId;
        }

        if (productEvalId > 0)
        {
            var productEval = db.Set<ProductEvalEntity>().Find(productEvalId);
            if (productEval == null)
            {
                throw new ValidationException($"指定的试用目录#{productEvalId}不存在!");
            }

            collegeId = productEval.CollegeId;
            var college = db.Set<CollegeEntity>().Find(collegeId);
            if (college == null)
            {
                throw new ValidationException($"指定学校#{collegeId}不存在!");
            }

            collegeName = college.Name;

            productId = productEval.ProductId;
            var product = db.Set<ProductEntity>().Find(productId);
            if (product == null)
            {
                throw new ValidationException($"指定产品#{productId}不存在!");
            }

            productName = product.Name;
        }

        var model = new ProductEvalEntryDetailModel {
            ProductEvalEntryId = productEvalEntryId,
            ProductEvalId = productEvalId,
            CollegeId = collegeId,
            CollegeName = collegeName,
            ProductId = productId,
            ProductName = productName,
            BeginDate = DateTime.Today.ToString("yyyy-MM-dd"),
            EndDate = DateTime.Today.ToString("yyyy-MM-dd"),
            Description = "",
        };

        if (productEvalEntryId > 0)
        {
            var productEvalEntry = db.Set<ProductEvalEntryEntity>().Find(productEvalEntryId);
            if (productEvalEntry == null)
            {
                throw new ValidationException($"指定的试用目录明细#{productEvalEntryId}不存在!");
            }

            model = new ProductEvalEntryDetailModel {
                ProductEvalEntryId = productEvalEntry.Id,
                ProductEvalId = productEvalEntry.ProductEvalId,
                CollegeId = collegeId,
                CollegeName = collegeName,
                ProductId = productId,
                ProductName = productName,
                BeginDate = productEvalEntry.BeginDate.ToString("yyyy-MM-dd"),
                EndDate = productEvalEntry.EndDate.ToString("yyyy-MM-dd"),
                Description = productEvalEntry.Description,
            };
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
        long productEvalId = 0;
        long collegeId = model.CollegeId;
        long productId = model.ProductId;

        if (model.ProductEvalEntryId <= 0)
        {
            if (model.ProductEvalId > 0)
            {
                productEvalId = model.ProductEvalId;
                var productEval = db.Find<ProductEvalEntity>(productEvalId);
                if (productEval == null)
                {
                    throw new ValidationException($"指定的试用目录#{productEvalId}不存在！");
                }
            }
            else
            {
                if (model.CollegeId <= 0)
                {
                    throw new ValidationException("学校不能为空!");
                }

                collegeId = model.CollegeId;
                var college = db.Set<CollegeEntity>().Find(collegeId);
                if (college == null)
                {
                    throw new ValidationException($"指定的学校#{collegeId}不存在!");
                }

                if (model.ProductId <= 0)
                {
                    throw new ValidationException("产品不能为空!");
                }

                productId = model.ProductId;
                var product = db.Find<ProductEntity>(productId);
                if (product == null)
                {
                    throw new ValidationException($"指定的产品#{productId}不存在!");
                }
            }
        }

        ValidationUtil.Validate<ProductEvalEntryDetailModelValidator, ProductEvalEntryDetailModel>(model);

        long loginUserId = userLoginService.GetLoginUserId();

        long productEvalEntryId = model.ProductEvalEntryId;
        bool isCreate = productEvalEntryId == 0;

        ProductEvalEntryEntity? productEvalEntry;
        if (isCreate == false)
        {
            productEvalEntry = db.Set<ProductEvalEntryEntity>().Find(productEvalEntryId);
            if (productEvalEntry == null)
            {
                throw new ValidationException($"指定的试用目录明细#{productEvalEntryId}不存在!");
            }
        }
        else
        {
            productEvalEntry = new ProductEvalEntryEntity();

            if (productEvalId > 0)
            {
                productEvalEntry.ProductEvalId = productEvalId;
            }
            else
            {
                var productEval = db.Set<ProductEvalEntity>()
                    .Where(e => e.CollegeId == collegeId && e.ProductId == productId)
                    .Take(1)
                    .SingleOrDefault();
                if (productEval != null)
                {
                    throw new ValidationException($"学校#{collegeId}和产品#{productId}对应的试用目录已经存在!");
                }

                // 先创建试用目录
                productEvalId = productEvalService.Create(collegeId, productId, loginUserId);
                productEvalEntry.ProductEvalId = productEvalId;
            }
        }

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

        productEvalId = productEvalEntry.ProductEvalId; // 从保存的实体里面再取一下, 肯定没错, 前面的路径里面有没对这个变量设值的情形
        productEvalService.UpdateProductEvalByLatestData(productEvalId);

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
        string entryActionDesc = $"#{productEvalEntry.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductEvalEntryDelete, entryActionDesc);

        // 删除最后一个明细数据的时候，删除主数据
        bool productEvalDeleted = false;
        long productEvalId = productEvalEntry.ProductEvalId;
        var productEvalEntryExists = db.Set<ProductEvalEntryEntity>()
            .Where(e => e.ProductEvalId == productEvalId)
            .Take(1)
            .SingleOrDefault();
        if (productEvalEntryExists == null)
        {
            // 如果最后一个明细也不存在了， 就删除主数据
            var productEval = db.Set<ProductEvalEntity>().Find(productEvalId);
            if (productEval == null)
            {
                // 一般肯定存在的
                throw new ValidationException($"指定的试用目录#{productEvalId}不存在!");
            }

            db.Remove<ProductEvalEntity>(productEval);
            db.SaveChanges();

            string actionDesc = $"#{productEval.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductEvalDelete, actionDesc);

            productEvalDeleted = true;
        }

        db.SaveChanges();

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
            Data = new Dictionary<string, object> {
                ["productEvalDeleted"] = productEvalDeleted
            },
        });
    }
}