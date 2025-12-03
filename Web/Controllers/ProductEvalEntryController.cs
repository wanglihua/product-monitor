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
using Service.ProductEval;
using Web.Models.ProductEvalEntry;

namespace Web.Controllers;

[Auth(PassRes.ProjectManage.ProductEval)]
public class ProductEvalEntryController(
    Db db,
    UserLoginService userLoginService,
    ActionLogService actionLogService,
    ProductEvalService productEvalService) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private class ProductEvalEntryJoinItem
    {
        public ProductEvalEntryEntity ProductEvalEntry { get; set; } = new();

        public UserEntity User { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/product-eval-entry/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductEvalEntryListParam param)
    {
        ValidationUtil.Validate<ProductEvalEntryListParamValidator, ProductEvalEntryListParam>(param);

        IQueryable<ProductEvalEntryJoinItem> query = db.ProductEvalEntry
            .Join(db.User,
                pse => pse.UserId,
                u => u.Id,
                (pse, u) => new ProductEvalEntryJoinItem {
                    ProductEvalEntry = pse,
                    User = u,
                })
            .AsNoTracking();

        long productEvalId = param.ProductEvalId;
        var productEval = db.ProductEval.Find(productEvalId);
        if (productEval is null)
        {
            throw new ValidationException($"指定的试用目录#{productEvalId}不存在!");
        }

        long collegeId = productEval.CollegeId;
        var college = db.College.Find(collegeId);
        if (college is null)
        {
            throw new ValidationException($"指定学校#{collegeId}不存在!");
        }

        string collegeName = college.Name;

        long productId = productEval.ProductId;
        var product = db.Product.Find(productId);
        if (product is null)
        {
            throw new ValidationException($"指定产品#{productId}不存在!");
        }

        string productName = product.Name;

        query = query.Where(p => p.ProductEvalEntry.ProductEvalId == productEvalId);

        int productEvalEntryCount = query.Count();

        // 排序字段
        Expression<Func<ProductEvalEntryJoinItem, object>> sortField = e => e.ProductEvalEntry.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(ProductEvalEntryListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductEvalEntry.CreateTime;
            }

            if (string.Compare(sortFieldParam, nameof(ProductEvalEntryListItem.UpdateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductEvalEntry.UpdateTime;
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
            .Select(e => new {
                Id = e.ProductEvalEntry.Id,
                ProductEvalId = e.ProductEvalEntry.ProductEvalId,
                TotalDays = e.ProductEvalEntry.TotalDays,
                BeginDate = e.ProductEvalEntry.BeginDate,
                EndDate = e.ProductEvalEntry.EndDate,
                Description = e.ProductEvalEntry.Description,
                UserName = e.User.UserName,
                CreateTime = e.ProductEvalEntry.CreateTime,
                UpdateTime = e.ProductEvalEntry.UpdateTime,
            })
            .ToList();

        var productEvalEntryItemList = productEvalEntryList
            .Select(e => new ProductEvalEntryListItem {
                Id = e.Id,
                ProductEvalId = e.ProductEvalId,
                TotalDays = e.TotalDays,
                BeginDate = e.BeginDate.ToString("yyyy-MM-dd"),
                EndDate = e.EndDate.ToString("yyyy-MM-dd"),
                Description = e.Description,
                UserName = e.UserName,
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
            var productEvalEntry = db.ProductEvalEntry.Find(productEvalEntryId);
            if (productEvalEntry is null)
            {
                throw new ValidationException($"指定的试用目录明细#{productEvalEntryId}不存在!");
            }

            productEvalId = productEvalEntry.ProductEvalId;
        }

        if (productEvalId > 0)
        {
            var productEval = db.ProductEval.Find(productEvalId);
            if (productEval is null)
            {
                throw new ValidationException($"指定的试用目录#{productEvalId}不存在!");
            }

            collegeId = productEval.CollegeId;
            var college = db.College.Find(collegeId);
            if (college is null)
            {
                throw new ValidationException($"指定学校#{collegeId}不存在!");
            }

            collegeName = college.Name;

            productId = productEval.ProductId;
            var product = db.Product.Find(productId);
            if (product is null)
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
            SalesManager = "",
            ContactTeacher = "",
            ContactMobile = "",
            Description = "",
        };

        if (productEvalEntryId > 0)
        {
            var productEvalEntry = db.ProductEvalEntry.Find(productEvalEntryId);
            if (productEvalEntry is null)
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
                SalesManager = productEvalEntry.SalesManager,
                ContactTeacher = productEvalEntry.ContactTeacher,
                ContactMobile = productEvalEntry.ContactMobile,
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
                if (productEval is null)
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
                var college = db.College.Find(collegeId);
                if (college is null)
                {
                    throw new ValidationException($"指定的学校#{collegeId}不存在!");
                }

                if (model.ProductId <= 0)
                {
                    throw new ValidationException("产品不能为空!");
                }

                productId = model.ProductId;
                var product = db.Find<ProductEntity>(productId);
                if (product is null)
                {
                    throw new ValidationException($"指定的产品#{productId}不存在!");
                }
            }
        }

        ValidationUtil.Validate<ProductEvalEntryDetailModelValidator, ProductEvalEntryDetailModel>(model);

        var beginDate = DateTime.Parse(model.BeginDate).Date;
        var endDate = DateTime.Parse(model.EndDate).Date;

        if (beginDate > endDate)
        {
            throw new ValidationException("开始时间应该在结束时间之前!");
        }

        long loginUserId = userLoginService.GetLoginUserId();

        long productEvalEntryId = model.ProductEvalEntryId;
        bool isCreate = productEvalEntryId == 0;

        ProductEvalEntryEntity? productEvalEntry;
        if (isCreate == false)
        {
            productEvalEntry = db.ProductEvalEntry.Find(productEvalEntryId);
            if (productEvalEntry is null)
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
                var productEval = db.ProductEval.AsNoTracking()
                    .Where(e => e.CollegeId == collegeId && e.ProductId == productId)
                    .OrderBy(e => e.CollegeId)
                    .Take(1)
                    .SingleOrDefault();
                if (productEval is not null)
                {
                    throw new ValidationException($"学校#{collegeId}和产品#{productId}对应的试用目录已经存在!");
                }

                // 先创建试用目录
                productEvalId = productEvalService.Create(collegeId, productId, loginUserId);
                productEvalEntry.ProductEvalId = productEvalId;
            }
        }

        productEvalEntry.BeginDate = beginDate;
        productEvalEntry.EndDate = endDate;

        var totalDays = (int)((endDate - beginDate).TotalDays) + 1;
        if (totalDays < 0)
        {
            throw new ValidationException("累计天数不能小于0!");
        }

        productEvalEntry.TotalDays = totalDays;
        productEvalEntry.SalesManager = model.SalesManager ?? "";
        productEvalEntry.ContactTeacher = model.ContactTeacher ?? "";
        productEvalEntry.ContactMobile = model.ContactMobile ?? "";
        productEvalEntry.Description = model.Description ?? "";
        productEvalEntry.UserId = loginUserId;

        if (isCreate == true)
        {
            db.ProductEvalEntry.Add(productEvalEntry);
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
        var productEvalEntry = db.ProductEvalEntry.Find(productEvalEntryId);
        if (productEvalEntry is null)
        {
            throw new ValidationException($"指定的试用目录明细#{productEvalEntryId}不存在!");
        }

        db.ProductEvalEntry.Remove(productEvalEntry);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string entryActionDesc = $"#{productEvalEntry.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductEvalEntryDelete, entryActionDesc);

        // 删除最后一个明细数据的时候，删除主数据
        bool productEvalDeleted = false;
        long productEvalId = productEvalEntry.ProductEvalId;
        var productEvalEntryExists = db.ProductEvalEntry.AsNoTracking()
            .Where(e => e.ProductEvalId == productEvalId)
            .OrderBy(e => e.ProductEvalId)
            .Take(1)
            .SingleOrDefault();
        if (productEvalEntryExists is null)
        {
            // 如果最后一个明细也不存在了， 就删除主数据
            var productEval = db.ProductEval.Find(productEvalId);
            if (productEval is null)
            {
                // 一般肯定存在的
                throw new ValidationException($"指定的试用目录#{productEvalId}不存在!");
            }

            db.ProductEval.Remove(productEval);
            db.SaveChanges();

            string actionDesc = $"#{productEval.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductEvalDelete, actionDesc);

            productEvalService.UpdateProductEvalByLatestData(productEvalId);

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