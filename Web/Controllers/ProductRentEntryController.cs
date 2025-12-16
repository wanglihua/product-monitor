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
using Service.ProductRent;
using Web.Models.ProductRentEntry;

namespace Web.Controllers;

[Auth(PassRes.ProjectManage.ProductRent)]
public class ProductRentEntryController(
    Db db,
    UserLoginService userLoginService,
    UserProvinceService userProvinceService,
    ActionLogService actionLogService,
    ProductRentService productRentService) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private class ProductRentEntryJoinItem
    {
        public ProductRentEntryEntity ProductRentEntry { get; set; } = new();

        public UserEntity User { get; set; } = new();

        public ProductRentEntity ProductRent { get; set; } = new();

        public CollegeEntity College { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/product-rent-entry/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductRentEntryListParam param)
    {
        ValidationUtil.Validate<ProductRentEntryListParamValidator, ProductRentEntryListParam>(param);

        long loginUserId = userLoginService.GetLoginUserId();
        bool isAdminLogin = userLoginService.IsAdmin(loginUserId);

        long productRentId = param.ProductRentId;
        var productRent = db.ProductRent.Find(productRentId);
        if (productRent is null)
        {
            throw new ValidationException($"指定的租赁目录#{productRentId}不存在!");
        }

        long collegeId = productRent.CollegeId;
        var college = db.College.Find(collegeId);
        if (college is null)
        {
            throw new ValidationException($"指定学校#{collegeId}不存在!");
        }

        string collegeName = college.Name;

        long productId = productRent.ProductId;
        var product = db.Product.Find(productId);
        if (product is null)
        {
            throw new ValidationException($"指定产品#{productId}不存在!");
        }

        string productName = product.Name;

        IQueryable<ProductRentEntryJoinItem> query = db.ProductRentEntry
            .Join(db.User,
                pse => pse.UserId,
                u => u.Id,
                (pse, u) => new ProductRentEntryJoinItem {
                    ProductRentEntry = pse,
                    User = u,
                })
            .Join(db.ProductRent,
                pse => pse.ProductRentEntry.ProductRentId,
                pr => pr.Id,
                (pse, pr) => new ProductRentEntryJoinItem {
                    ProductRentEntry = pse.ProductRentEntry,
                    User = pse.User,
                    ProductRent = pr,
                })
            .Join(db.College,
                pse => pse.ProductRent.CollegeId,
                c => c.Id,
                (pse, c) => new ProductRentEntryJoinItem {
                    ProductRentEntry = pse.ProductRentEntry,
                    User = pse.User,
                    ProductRent = pse.ProductRent,
                    College = c
                })
            .AsNoTracking();

        if (isAdminLogin == false)
        {
            var userProvinceList = userProvinceService.GetUserProvinceList(loginUserId);
            query = query.Where(e => userProvinceList.Contains(e.College.Province));
        }

        query = query.Where(p => p.ProductRentEntry.ProductRentId == productRentId);

        int productRentEntryCount = query.Count();

        // 排序字段
        Expression<Func<ProductRentEntryJoinItem, object>> sortField = e => e.ProductRentEntry.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(ProductRentEntryListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductRentEntry.CreateTime;
            }

            if (string.Compare(sortFieldParam, nameof(ProductRentEntryListItem.UpdateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductRentEntry.UpdateTime;
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
            .Select(e => new {
                Id = e.ProductRentEntry.Id,
                TotalDays = e.ProductRentEntry.TotalDays,
                BeginDate = e.ProductRentEntry.BeginDate,
                EndDate = e.ProductRentEntry.EndDate,
                Description = e.ProductRentEntry.Description,
                UserName = e.User.UserName,
                CreateTime = e.ProductRentEntry.CreateTime,
                UpdateTime = e.ProductRentEntry.UpdateTime,
            })
            .ToList();

        var productRentEntryItemList = productRentEntryList
            .Select(e => new ProductRentEntryListItem {
                Id = e.Id,
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
                ["productRentEntryList"] = productRentEntryItemList,
                ["productRentEntryCount"] = productRentEntryCount,
                ["collegeName"] = collegeName,
                ["productName"] = productName,
            },
        });
    }

    [HttpPost]
    [Route("/api/product-rent-entry/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] ProductRentEntryDetailParam param)
    {
        ValidationUtil.Validate<ProductRentEntryDetailParamValidator, ProductRentEntryDetailParam>(param);

        long loginUserId = userLoginService.GetLoginUserId();
        bool isAdminLogin = userLoginService.IsAdmin(loginUserId);

        long productRentEntryId = param.ProductRentEntryId;

        long collegeId = 0;
        string collegeName = "";
        long productId = 0;
        string productName = "";

        long productRentId = param.ProductRentId;
        if (productRentEntryId > 0)
        {
            var productRentEntry = db.ProductRentEntry.Find(productRentEntryId);
            if (productRentEntry is null)
            {
                throw new ValidationException($"指定的租赁目录明细#{productRentEntryId}不存在!");
            }

            productRentId = productRentEntry.ProductRentId;
        }

        productRentService.CheckDataPrivilege(productRentId, loginUserId, isAdminLogin);

        var productRent = db.ProductRent.Find(productRentId);
        if (productRent is null)
        {
            throw new ValidationException($"指定的租赁目录#{productRentId}不存在!");
        }

        collegeId = productRent.CollegeId;
        var college = db.College.Find(collegeId);
        if (college is null)
        {
            throw new ValidationException($"指定学校#{collegeId}不存在!");
        }

        collegeName = college.Name;

        productId = productRent.ProductId;
        var product = db.Product.Find(productId);
        if (product is null)
        {
            throw new ValidationException($"指定产品#{productId}不存在!");
        }

        productName = product.Name;

        var model = new ProductRentEntryDetailModel {
            ProductRentEntryId = productRentEntryId,
            ProductRentId = productRentId,
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

        if (productRentEntryId > 0)
        {
            var productRentEntry = db.ProductRentEntry.Find(productRentEntryId);
            if (productRentEntry is null)
            {
                throw new ValidationException($"指定的租赁目录明细#{productRentEntryId}不存在!");
            }

            model = new ProductRentEntryDetailModel {
                ProductRentEntryId = productRentEntry.Id,
                ProductRentId = productRentEntry.ProductRentId,
                CollegeId = collegeId,
                CollegeName = collegeName,
                ProductId = productId,
                ProductName = productName,
                BeginDate = productRentEntry.BeginDate.ToString("yyyy-MM-dd"),
                EndDate = productRentEntry.EndDate.ToString("yyyy-MM-dd"),
                SalesManager = productRentEntry.SalesManager,
                ContactTeacher = productRentEntry.ContactTeacher,
                ContactMobile = productRentEntry.ContactMobile,
                Description = productRentEntry.Description,
            };
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
        long productRentId = 0;
        long collegeId = model.CollegeId;
        long productId = model.ProductId;

        if (model.ProductRentEntryId <= 0)
        {
            if (model.ProductRentId > 0)
            {
                productRentId = model.ProductRentId;
                var productRent = db.ProductRent.Find(productRentId);
                if (productRent is null)
                {
                    throw new ValidationException($"指定的租赁目录#{productRentId}不存在！");
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
                var product = db.Product.Find(productId);
                if (product is null)
                {
                    throw new ValidationException($"指定的产品#{productId}不存在!");
                }
            }
        }

        ValidationUtil.Validate<ProductRentEntryDetailModelValidator, ProductRentEntryDetailModel>(model);

        var beginDate = DateTime.Parse(model.BeginDate).Date;
        var endDate = DateTime.Parse(model.EndDate).Date;

        if (beginDate > endDate)
        {
            throw new ValidationException("开始时间应该在结束时间之前!");
        }

        long loginUserId = userLoginService.GetLoginUserId();
        bool isAdminLogin = userLoginService.IsAdmin(loginUserId);

        long productRentEntryId = model.ProductRentEntryId;
        bool isCreate = productRentEntryId == 0;

        ProductRentEntryEntity? productRentEntry;
        if (isCreate == false)
        {
            productRentEntry = db.ProductRentEntry.Find(productRentEntryId);
            if (productRentEntry is null)
            {
                throw new ValidationException($"指定的租赁目录明细#{productRentEntryId}不存在!");
            }
        }
        else
        {
            productRentEntry = new ProductRentEntryEntity();

            if (productRentId > 0)
            {
                productRentEntry.ProductRentId = productRentId;
            }
            else
            {
                var productRent = db.ProductRent.AsNoTracking()
                    .Where(e => e.CollegeId == collegeId && e.ProductId == productId)
                    .OrderBy(e => e.CollegeId)
                    .Take(1)
                    .SingleOrDefault();
                if (productRent is not null)
                {
                    throw new ValidationException($"学校#{collegeId}和产品#{productId}对应的租赁目录已经存在!");
                }

                // 先创建租赁目录
                productRentId = productRentService.Create(collegeId, productId, loginUserId);
                productRentEntry.ProductRentId = productRentId;
            }
        }

        long productRentBindId = productRentEntry.ProductRentId;
        productRentService.CheckDataPrivilege(productRentBindId, loginUserId, isAdminLogin);

        productRentEntry.BeginDate = beginDate;
        productRentEntry.EndDate = endDate;

        var totalDays = (int)((endDate - beginDate).TotalDays) + 1;
        if (totalDays < 0)
        {
            throw new ValidationException("累计天数不能小于0!");
        }

        productRentEntry.TotalDays = totalDays;

        productRentEntry.SalesManager = model.SalesManager ?? "";
        productRentEntry.ContactTeacher = model.ContactTeacher ?? "";
        productRentEntry.ContactMobile = model.ContactMobile ?? "";
        productRentEntry.Description = model.Description ?? "";
        productRentEntry.UserId = loginUserId;

        if (isCreate == true)
        {
            db.ProductRentEntry.Add(productRentEntry);
        }

        db.SaveChanges();

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

        productRentId = productRentEntry.ProductRentId; // 从保存的实体里面再取一下, 肯定没错, 前面的路径里面有没对这个变量设值的情形
        productRentService.UpdateProductRentByLatestData(productRentId);

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

        long loginUserId = userLoginService.GetLoginUserId();
        bool isAdminLogin = userLoginService.IsAdmin(loginUserId);

        long productRentEntryId = param.Id;
        var productRentEntry = db.ProductRentEntry.Find(productRentEntryId);
        if (productRentEntry is null)
        {
            throw new ValidationException($"指定的租赁目录明细#{productRentEntryId}不存在!");
        }

        long productRentId = productRentEntry.ProductRentId;
        productRentService.CheckDataPrivilege(productRentId, loginUserId, isAdminLogin);

        db.ProductRentEntry.Remove(productRentEntry);
        db.SaveChanges();

        string entryActionDesc = $"#{productRentEntry.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductRentEntryDelete, entryActionDesc);

        // 删除最后一个明细数据的时候，删除主数据
        bool productRentDeleted = false;
        var productRentEntryExists = db.ProductRentEntry.AsNoTracking()
            .Where(e => e.ProductRentId == productRentId)
            .OrderBy(e => e.ProductRentId)
            .Take(1)
            .SingleOrDefault();

        if (productRentEntryExists is not null)
        {
            productRentService.UpdateProductRentByLatestData(productRentId);
        }

        if (productRentEntryExists is null)
        {
            // 如果最后一个明细也不存在了， 就删除主数据
            var productRent = db.ProductRent.Find(productRentId);
            if (productRent is null)
            {
                // 一般肯定存在的
                throw new ValidationException($"指定的租赁目录#{productRentId}不存在!");
            }

            db.ProductRent.Remove(productRent);
            db.SaveChanges();

            string actionDesc = $"#{productRent.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductRentDelete, actionDesc);

            productRentDeleted = true;
        }

        db.SaveChanges();

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
            Data = new Dictionary<string, object> {
                ["productRentDeleted"] = productRentDeleted
            },
        });
    }
}