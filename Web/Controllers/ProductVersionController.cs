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
using Web.Models.ProductVersion;

namespace Web.Controllers;

[Auth(PassRes.BasicSetting.Product)]
public class ProductVersionController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly ProductService productService;
    private readonly ActionLogService actionLogService;

    public ProductVersionController(
        Db db,
        UserLoginService userLoginService,
        ProductService productService,
        ActionLogService actionLogService
    )
    {
        this.db = db;
        this.userLoginService = userLoginService;
        this.productService = productService;
        this.actionLogService = actionLogService;
    }

    private class ProductVersionJoinItem
    {
        public ProductVersionEntity ProductVersion { get; set; } = new();

        public ProductEntity Product { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/product-version/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductVersionListParam param)
    {
        ValidationUtil.Validate<ProductVersionListParamValidator, ProductVersionListParam>(param);

        long productId = param.ProductId;

        var product = db.Set<ProductEntity>().Find(productId);
        if (product == null)
        {
            throw new ValidationException($"指定的产品#{productId}不存在!");
        }

        IQueryable<ProductVersionJoinItem> query = db.Set<ProductVersionEntity>()
            .Join(db.Set<ProductEntity>(),
                pv => pv.ProductId,
                p => p.Id,
                (pv, p) => new ProductVersionJoinItem {
                    ProductVersion = pv,
                    Product = p,
                });

        query = query.Where(e => e.ProductVersion.ProductId == productId);

        int productVersionCount = query.Count();

        // 排序字段
        Expression<Func<ProductVersionJoinItem, object>> sortField = e => e.ProductVersion.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(ProductVersionListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductVersion.CreateTime;
            }

            if (string.Compare(sortFieldParam, nameof(ProductVersionListItem.UpdateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductVersion.UpdateTime;
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

        var productVersionList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var productVersionItemList = productVersionList
            .Select(e => new ProductVersionListItem {
                Id = e.ProductVersion.Id,
                ProductName = e.Product.Name,
                Version = e.ProductVersion.Version,
                VersionUpdateBase = e.ProductVersion.VersionUpdateBase,
                VersionUpdateDesc = e.ProductVersion.VersionUpdateDesc,
                VersionUpdateTime = e.ProductVersion.VersionUpdateTime.ToString("yyyy-MM-dd"),
                CreateTime = e.ProductVersion.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.ProductVersion.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["productVersionList"] = productVersionItemList,
                ["productVersionCount"] = productVersionCount,
                ["productName"] = product.Name,
            },
        });
    }

    [HttpPost]
    [Route("/api/product-version/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] ProductVersionDetailParam param)
    {
        ValidationUtil.Validate<ProductVersionDetailParamValidator, ProductVersionDetailParam>(param);

        long productId = param.ProductId;
        var product = db.Set<ProductEntity>().Find(productId);
        if (product == null)
        {
            throw new ValidationException($"指定的产品#{productId}不存在!");
        }

        string productName = product.Name;

        long productVersionId = param.ProductVersionId;

        var model = new ProductVersionDetailModel {
            Id = 0,
            ProductId = productId,
            Version = "",
            VersionUpdateBase = "",
            VersionUpdateDesc = "",
            VersionUpdateTime = DateTime.Now.ToString("yyyy-MM-dd"),
            ProductName = productName,
        };

        if (productVersionId > 0)
        {
            var productVersion = db.Set<ProductVersionEntity>().Find(productVersionId);
            if (productVersion != null)
            {
                model = new ProductVersionDetailModel {
                    Id = productVersion.Id,
                    ProductId = productVersion.ProductId,
                    Version = productVersion.Version,
                    VersionUpdateBase = productVersion.VersionUpdateBase,
                    VersionUpdateDesc = productVersion.VersionUpdateDesc,
                    VersionUpdateTime = productVersion.VersionUpdateTime.ToString("yyyy-MM-dd"),
                    ProductName = productName,
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
    [Route("/api/product-version/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] ProductVersionDetailModel model)
    {
        ValidationUtil.Validate<ProductVersionDetailModelValidator, ProductVersionDetailModel>(model);

        long productId = model.ProductId;
        var product = db.Set<ProductEntity>().Find(productId);
        if (product == null)
        {
            throw new ValidationException($"指定的产品目录#{productId}不存在!");
        }

        long productVersionId = model.Id;

        bool isCreate = productVersionId == 0;

        ProductVersionEntity? productVersion;
        if (isCreate == true)
        {
            productVersion = new ProductVersionEntity {
                ProductId = productId,
            };
        }
        else
        {
            productVersion = db.Set<ProductVersionEntity>().Find(productVersionId);
            if (productVersion == null)
            {
                throw new ValidationException($"指定的产品版本#{productVersionId}不存在!");
            }

            if (productVersion.ProductId != productId)
            {
                throw new ValidationException($"指定的产品版本#{productVersionId}与指定的产品#{productId}不匹配!");
            }
        }

        productVersion.Version = model.Version ?? "";
        productVersion.VersionUpdateBase = model.VersionUpdateBase ?? "";
        productVersion.VersionUpdateDesc = model.VersionUpdateDesc ?? "";
        productVersion.VersionUpdateTime = DateTime.Parse(model.VersionUpdateTime).Date;

        if (isCreate == true)
        {
            db.Add<ProductVersionEntity>(productVersion);
        }
        else
        {
            db.Update<ProductVersionEntity>(productVersion);
        }

        db.SaveChanges();

        productService.UpdateLatestVersionData(productId);

        long loginUserId = userLoginService.GetLoginUserId();
        if (isCreate == true)
        {
            string actionDesc = $"产品{product.Name} 版本{productVersion.Version}#{productVersion.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductVersionCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"产品{product.Name} 版本{productVersion.Version}#{productVersion.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductVersionUpdate, actionDesc);
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/product-version/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long productVersionId = param.Id;
        var productVersion = db.Set<ProductVersionEntity>().Find(productVersionId);
        if (productVersion == null)
        {
            throw new ValidationException($"指定的产品版本#{productVersionId}不存在!");
        }

        long productId = productVersion.ProductId;
        var product = db.Set<ProductEntity>().Find(productId);
        if (product == null)
        {
            throw new ValidationException($"指定的产品目录#{productId}不存在!");
        }

        db.Remove<ProductVersionEntity>(productVersion);
        db.SaveChanges();

        productService.UpdateLatestVersionData(productId);

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"产品{product.Name} 版本{productVersion.Version}#{productVersion.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductVersionDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}