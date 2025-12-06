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
using Service.ProductSale;
using Web.Models.ProductSaleEntry;

namespace Web.Controllers;

[Auth(PassRes.ProjectManage.ProductSale)]
public class ProductSaleEntryController(
    Db db,
    UserLoginService userLoginService,
    ActionLogService actionLogService,
    ProductSaleService productSaleService) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private class ProductSaleEntryJoinItem
    {
        public ProductSaleEntryEntity ProductSaleEntry { get; set; } = new();

        public UserEntity User { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/product-sale-entry/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductSaleEntryListParam param)
    {
        ValidationUtil.Validate<ProductSaleEntryListParamValidator, ProductSaleEntryListParam>(param);

        long productSaleId = param.ProductSaleId;
        var productSale = db.ProductSale.Find(productSaleId);
        if (productSale is null)
        {
            throw new ValidationException($"指定的销售目录#{productSaleId}不存在!");
        }

        long collegeId = productSale.CollegeId;
        var college = db.College.Find(collegeId);
        if (college is null)
        {
            throw new ValidationException($"指定学校#{collegeId}不存在!");
        }

        string collegeName = college.Name;

        long productId = productSale.ProductId;
        var product = db.Product.Find(productId);
        if (product is null)
        {
            throw new ValidationException($"指定产品#{productId}不存在!");
        }

        string productName = product.Name;

        IQueryable<ProductSaleEntryJoinItem> query = db.ProductSaleEntry
            .Join(db.User,
                pse => pse.UserId,
                u => u.Id,
                (pse, u) => new ProductSaleEntryJoinItem {
                    ProductSaleEntry = pse,
                    User = u,
                })
            .AsNoTracking();

        query = query.Where(p => p.ProductSaleEntry.ProductSaleId == productSaleId);


        if (string.IsNullOrWhiteSpace(param.VersionQuery) == false)
        {
            string versionQuery = param.VersionQuery.Trim();
            query = query.Where(p => p.ProductSaleEntry.Version.Contains(versionQuery));
        }

        int productSaleEntryCount = query.Count();

        // 排序字段
        Expression<Func<ProductSaleEntryJoinItem, object>> sortField = e => e.ProductSaleEntry.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(ProductSaleEntryListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductSaleEntry.CreateTime;
            }

            if (string.Compare(sortFieldParam, nameof(ProductSaleEntryListItem.UpdateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductSaleEntry.UpdateTime;
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
            .Select(e => new {
                Id = e.ProductSaleEntry.Id,
                ProductSaleId = e.ProductSaleEntry.ProductSaleId,
                Version = e.ProductSaleEntry.Version,
                SaleDate = e.ProductSaleEntry.SaleDate,
                ServiceExpireDate = e.ProductSaleEntry.ServiceExpireDate,
                Description = e.ProductSaleEntry.Description,
                UserName = e.User.UserName,
                CreateTime = e.ProductSaleEntry.CreateTime,
                UpdateTime = e.ProductSaleEntry.UpdateTime,
            })
            .ToList();

        var productSaleEntryItemList = productSaleEntryList
            .Select(e => new ProductSaleEntryListItem {
                Id = e.Id,
                ProductSaleId = e.ProductSaleId,
                Version = e.Version,
                SaleDate = e.SaleDate.ToString("yyyy-MM-dd"),
                ServiceExpireDate = e.ServiceExpireDate.ToString("yyyy-MM-dd"),
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
                ["productSaleEntryList"] = productSaleEntryItemList,
                ["productSaleEntryCount"] = productSaleEntryCount,
                ["collegeName"] = collegeName,
                ["productName"] = productName,
            },
        });
    }

    [HttpPost]
    [Route("/api/product-sale-entry/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] ProductSaleEntryDetailParam param)
    {
        long productSaleEntryId = param.ProductSaleEntryId;

        long collegeId = 0;
        string collegeName = "";
        long productId = 0;
        string productName = "";

        long productSaleId = param.ProductSaleId;
        if (productSaleEntryId > 0)
        {
            var productSaleEntry = db.ProductSaleEntry.Find(productSaleEntryId);
            if (productSaleEntry is null)
            {
                throw new ValidationException($"指定的销售目录明细#{productSaleEntryId}不存在!");
            }

            productSaleId = productSaleEntry.ProductSaleId;
        }

        if (productSaleId > 0)
        {
            var productSale = db.ProductSale.Find(productSaleId);
            if (productSale is null)
            {
                throw new ValidationException($"指定的销售目录#{productSaleId}不存在!");
            }

            collegeId = productSale.CollegeId;
            var college = db.College.Find(collegeId);
            if (college is null)
            {
                throw new ValidationException($"指定学校#{collegeId}不存在!");
            }

            collegeName = college.Name;

            productId = productSale.ProductId;
            var product = db.Product.Find(productId);
            if (product is null)
            {
                throw new ValidationException($"指定产品#{productId}不存在!");
            }

            productName = product.Name;
        }

        var model = new ProductSaleEntryDetailModel {
            ProductSaleEntryId = productSaleEntryId,
            ProductSaleId = productSaleId,
            CollegeId = collegeId,
            CollegeName = collegeName,
            ProductId = productId,
            ProductName = productName,
            Version = "",
            SaleDate = DateTime.Today.ToString("yyyy-MM-dd"),
            ServiceExpireDate = DateTime.Today.ToString("yyyy-MM-dd"),
            SalesManager = "",
            ContactTeacher = "",
            ContactMobile = "",
            Description = "",
        };

        if (productSaleEntryId > 0)
        {
            var productSaleEntry = db.ProductSaleEntry.Find(productSaleEntryId);
            if (productSaleEntry is null)
            {
                throw new ValidationException($"指定的销售目录明细#{productSaleEntryId}不存在!");
            }

            model = new ProductSaleEntryDetailModel {
                ProductSaleEntryId = productSaleEntry.Id,
                ProductSaleId = productSaleEntry.ProductSaleId,
                CollegeId = collegeId,
                CollegeName = collegeName,
                ProductId = productId,
                ProductName = productName,
                Version = productSaleEntry.Version,
                SaleDate = productSaleEntry.SaleDate.ToString("yyyy-MM-dd"),
                ServiceExpireDate = productSaleEntry.ServiceExpireDate.ToString("yyyy-MM-dd"),
                SalesManager = productSaleEntry.SalesManager,
                ContactTeacher = productSaleEntry.ContactTeacher,
                ContactMobile = productSaleEntry.ContactMobile,
                Description = productSaleEntry.Description,
            };
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
        long productSaleId = 0;
        long collegeId = model.CollegeId;
        long productId = model.ProductId;

        if (model.ProductSaleEntryId <= 0)
        {
            if (model.ProductSaleId > 0)
            {
                productSaleId = model.ProductSaleId;
                var productSale = db.ProductSale.Find(productSaleId);
                if (productSale is null)
                {
                    throw new ValidationException($"指定的销售目录#{productSaleId}不存在！");
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

        ValidationUtil.Validate<ProductSaleEntryDetailModelValidator, ProductSaleEntryDetailModel>(model);

        long loginUserId = userLoginService.GetLoginUserId();

        long productSaleEntryId = model.ProductSaleEntryId;
        bool isCreate = productSaleEntryId == 0;

        ProductSaleEntryEntity? productSaleEntry;
        if (isCreate == false)
        {
            productSaleEntry = db.ProductSaleEntry.Find(productSaleEntryId);
            if (productSaleEntry is null)
            {
                throw new ValidationException($"指定的销售目录明细#{productSaleEntryId}不存在!");
            }
        }
        else
        {
            productSaleEntry = new ProductSaleEntryEntity();

            if (productSaleId > 0)
            {
                productSaleEntry.ProductSaleId = productSaleId;
            }
            else
            {
                var productSale = db.ProductSale.AsNoTracking()
                    .Where(e => e.CollegeId == collegeId && e.ProductId == productId)
                    .OrderBy(e => e.CollegeId)
                    .Take(1)
                    .SingleOrDefault();
                if (productSale is not null)
                {
                    throw new ValidationException($"学校#{collegeId}和产品#{productId}对应的销售目录已经存在!");
                }

                // 先创建销售目录
                productSaleId = productSaleService.Create(collegeId, productId, loginUserId);
                productSaleEntry.ProductSaleId = productSaleId;
            }
        }

        productSaleEntry.Version = model.Version ?? "";
        productSaleEntry.SaleDate = DateTime.Parse(model.SaleDate).Date;
        productSaleEntry.ServiceExpireDate = DateTime.Parse(model.ServiceExpireDate).Date;
        productSaleEntry.SalesManager = model.SalesManager ?? "";
        productSaleEntry.ContactTeacher = model.ContactTeacher ?? "";
        productSaleEntry.ContactMobile = model.ContactMobile ?? "";
        productSaleEntry.Description = model.Description ?? "";
        productSaleEntry.UserId = loginUserId;

        if (isCreate == true)
        {
            db.ProductSaleEntry.Add(productSaleEntry);
        }

        db.SaveChanges();

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

        productSaleId = productSaleEntry.ProductSaleId; // 从保存的实体里面再取一下, 肯定没错, 前面的路径里面有没对这个变量设值的情形
        productSaleService.UpdateProductSaleByLatestData(productSaleId);

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
        var productSaleEntry = db.ProductSaleEntry.Find(productSaleEntryId);
        if (productSaleEntry is null)
        {
            throw new ValidationException($"指定的销售目录明细#{productSaleEntryId}不存在!");
        }

        db.ProductSaleEntry.Remove(productSaleEntry);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string entryActionDesc = $"#{productSaleEntry.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductSaleEntryDelete, entryActionDesc);

        // 删除最后一个明细数据的时候，删除主数据
        bool productSaleDeleted = false;
        long productSaleId = productSaleEntry.ProductSaleId;
        var productSaleEntryExists = db.ProductSaleEntry.AsNoTracking()
            .Where(e => e.ProductSaleId == productSaleId)
            .OrderBy(e => e.ProductSaleId)
            .Take(1)
            .SingleOrDefault();
        if (productSaleEntryExists is null)
        {
            // 如果最后一个明细也不存在了， 就删除主数据
            var productSale = db.ProductSale.Find(productSaleId);
            if (productSale is null)
            {
                // 一般肯定存在的
                throw new ValidationException($"指定的销售目录#{productSaleId}不存在!");
            }

            db.ProductSale.Remove(productSale);
            db.SaveChanges();

            string actionDesc = $"#{productSale.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductSaleDelete, actionDesc);

            productSaleService.UpdateProductSaleByLatestData(productSaleId);

            productSaleDeleted = true;
        }

        db.SaveChanges();

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
            Data = new Dictionary<string, object> {
                ["productSaleDeleted"] = productSaleDeleted
            },
        });
    }
}