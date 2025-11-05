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
using Web.Models.Product;

namespace Web.Controllers;

[Auth(PassRes.BasicSetting.Product)]
public class ProductController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly ProductService productService;
    private readonly ActionLogService actionLogService;

    public ProductController(
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

    [HttpPost]
    [Route("/api/product/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductListParam param)
    {
        IQueryable<ProductEntity> query = db.Set<ProductEntity>();

        if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        {
            string nameQuery = param.NameQuery.Trim();
            query = query.Where(p => p.Name.Contains(nameQuery));
        }

        if (string.IsNullOrWhiteSpace(param.EnglishNameQuery) == false)
        {
            string englishNameQuery = param.EnglishNameQuery.Trim();
            query = query.Where(p => p.EnglishName.Contains(englishNameQuery));
        }

        int productCount = query.Count();

        // 排序字段
        Expression<Func<ProductEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();

            if (sortFieldParam == nameof(ProductListItem.CreateTime).ToUpper())
            {
                sortField = e => e.CreateTime;
            }

            if (sortFieldParam == nameof(ProductListItem.UpdateTime).ToUpper())
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

        var productList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var productItemList = productList
            .Select(e => new ProductListItem {
                Id = e.Id,
                Name = e.Name,
                EnglishName = e.EnglishName,
                VersionCount = e.VersionCount,
                LatestVersion = e.LatestVersion,
                VersionUpdateTime = e.VersionUpdateTime?.ToString("yyyy-MM-dd") ?? "",
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["productList"] = productItemList,
                ["productCount"] = productCount,
            },
        });
    }

    [HttpPost]
    [Route("/api/product/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        long productId = param.Id;

        var model = new ProductDetailModel {
            Id = 0,
            Name = "",
            EnglishName = "",
        };

        if (productId > 0)
        {
            var product = db.Set<ProductEntity>().Find(productId);
            if (product != null)
            {
                model = new ProductDetailModel {
                    Id = product.Id,
                    Name = product.Name,
                    EnglishName = product.EnglishName,
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
    [Route("/api/product/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] ProductDetailModel model)
    {
        ValidationUtil.Validate<ProductDetailModelValidator, ProductDetailModel>(model);

        long productId = model.Id;
        bool isCreate = productId == 0;

        ProductEntity? product;
        if (isCreate == true)
        {
            product = new ProductEntity();
        }
        else
        {
            product = db.Set<ProductEntity>().Find(productId);
            if (product == null)
            {
                throw new ValidationException($"指定的产品目录#{productId}不存在!");
            }
        }

        product.Name = model.Name ?? "";
        product.EnglishName = model.EnglishName ?? "";
        product.VersionCount = 0;
        product.LatestVersion = "";
        product.VersionUpdateTime = null;

        if (isCreate == true)
        {
            db.Add<ProductEntity>(product);
        }
        else
        {
            db.Update<ProductEntity>(product);
        }

        db.SaveChanges();

        productId = product.Id;

        long loginUserId = userLoginService.GetLoginUserId();
        if (isCreate == true)
        {
            string actionDesc = $"{product.Name}#{product.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"{product.Name}#{product.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductUpdate, actionDesc);
        }

        productService.UpdateLatestVersionData(productId);

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/product/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long productId = param.Id;
        var product = db.Set<ProductEntity>().Find(productId);
        if (product == null)
        {
            throw new ValidationException($"指定的产品目录#{productId}不存在!");
        }

        db.Remove<ProductEntity>(product);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"{product.Name}#{product.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}