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
    private readonly ProductSaleService productSaleService;

    public ProductSaleEntryController(
        Db db,
        UserLoginService userLoginService,
        ActionLogService actionLogService,
        ProductSaleService productSaleService
    )
    {
        this.db = db;
        this.userLoginService = userLoginService;
        this.actionLogService = actionLogService;
        this.productSaleService = productSaleService;
    }

    [HttpPost]
    [Route("/api/product-sale-entry/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductSaleEntryListParam param)
    {
        ValidationUtil.Validate<ProductSaleEntryListParamValidator, ProductSaleEntryListParam>(param);

        IQueryable<ProductSaleEntryEntity> query = db.Set<ProductSaleEntryEntity>();

        long productSaleId = param.ProductSaleId;
        query = query.Where(p => p.ProductSaleId == productSaleId);

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
            var productSaleEntry = db.Set<ProductSaleEntryEntity>().Find(productSaleEntryId);
            if (productSaleEntry == null)
            {
                throw new ValidationException($"指定的销售目录明细#{productSaleEntryId}不存在!");
            }

            productSaleId = productSaleEntry.ProductSaleId;
        }

        if (productSaleId > 0)
        {
            var productSale = db.Set<ProductSaleEntity>().Find(productSaleId);
            if (productSale == null)
            {
                throw new ValidationException($"指定的销售目录#{productSaleId}不存在!");
            }

            collegeId = productSale.CollegeId;
            var college = db.Set<CollegeEntity>().Find(collegeId);
            if (college == null)
            {
                throw new ValidationException($"指定学校#{collegeId}不存在!");
            }

            collegeName = college.Name;

            productId = productSale.ProductId;
            var product = db.Set<ProductEntity>().Find(productId);
            if (product == null)
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
            Description = "",
        };

        if (productSaleEntryId > 0)
        {
            var productSaleEntry = db.Set<ProductSaleEntryEntity>().Find(productSaleEntryId);
            if (productSaleEntry == null)
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
                var productSale = db.Find<ProductSaleEntity>(productSaleId);
                if (productSale == null)
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

        ValidationUtil.Validate<ProductSaleEntryDetailModelValidator, ProductSaleEntryDetailModel>(model);

        long loginUserId = userLoginService.GetLoginUserId();

        long productSaleEntryId = model.ProductSaleEntryId;
        bool isCreate = productSaleEntryId == 0;

        ProductSaleEntryEntity? productSaleEntry;
        if (isCreate == false)
        {
            productSaleEntry = db.Set<ProductSaleEntryEntity>().Find(productSaleEntryId);
            if (productSaleEntry == null)
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
                var productSale = db.Set<ProductSaleEntity>()
                    .Where(e => e.CollegeId == collegeId && e.ProductId == productId)
                    .Take(1)
                    .SingleOrDefault();
                if (productSale != null)
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
        var productSaleEntry = db.Set<ProductSaleEntryEntity>().Find(productSaleEntryId);
        if (productSaleEntry == null)
        {
            throw new ValidationException($"指定的销售目录明细#{productSaleEntryId}不存在!");
        }

        db.Remove<ProductSaleEntryEntity>(productSaleEntry);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string entryActionDesc = $"#{productSaleEntry.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductSaleEntryDelete, entryActionDesc);

        // 删除最后一个明细数据的时候，删除主数据
        long productSaleId = productSaleEntry.ProductSaleId;
        var productSaleEntryExists = db.Set<ProductSaleEntryEntity>()
            .Where(e => e.ProductSaleId == productSaleId)
            .Take(1)
            .SingleOrDefault();
        if (productSaleEntryExists == null)
        {
            // 如果最后一个明细也不存在了， 就删除主数据
            var productSale = db.Set<ProductSaleEntity>().Find(productSaleId);
            if (productSale == null)
            {
                // 一般肯定存在的
                throw new ValidationException($"指定的销售目录#{productSaleId}不存在!");
            }

            db.Remove<ProductSaleEntity>(productSale);
            db.SaveChanges();

            string actionDesc = $"#{productSale.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductSaleDelete, actionDesc);
        }

        db.SaveChanges();

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}