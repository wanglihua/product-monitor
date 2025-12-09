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
using Web.Models.ProductUrl;

namespace Web.Controllers;

[Auth(PassRes.BasicSetting.ProductUrl)]
public class ProductUrlController(
    Db db,
    UserLoginService userLoginService,
    ActionLogService actionLogService) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private class ProductUrlJoinItem
    {
        public ProductUrlEntity ProductUrl { get; set; } = new();

        public ProductEntity Product { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/product-url/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductUrlListParam param)
    {
        ValidationUtil.Validate<ProductUrlListParamValidator, ProductUrlListParam>(param);

        IQueryable<ProductUrlJoinItem> query = db.ProductUrl
            .Join(db.Product,
                pu => pu.ProductId,
                p => p.Id,
                (pu, p) => new ProductUrlJoinItem {
                    ProductUrl = pu,
                    Product = p,
                })
            .AsNoTracking();

        if (param.TypeQuery != ProductUrlType.None)
        {
            var typeQuery = param.TypeQuery;
            query = query.Where(e => e.ProductUrl.Type == typeQuery);
        }

        if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        {
            string nameQuery = param.NameQuery.Trim();
            query = query.Where(p => p.Product.Name.Contains(nameQuery));
        }

        int productUrlCount = query.Count();

        // 排序字段
        Expression<Func<ProductUrlJoinItem, object>> sortField = e => e.ProductUrl.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(ProductUrlListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductUrl.CreateTime;
            }

            if (string.Compare(sortFieldParam, nameof(ProductUrlListItem.UpdateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductUrl.UpdateTime;
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

        var productUrlList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .Select(e => new {
                Id = e.ProductUrl.Id,
                Name = e.Product.Name,
                Url = e.ProductUrl.Url,
                Type = e.ProductUrl.Type,
                LoginUser = e.ProductUrl.LoginUser,
                LoginPassword = e.ProductUrl.LoginPassword,
                CreateTime = e.ProductUrl.CreateTime,
                UpdateTime = e.ProductUrl.UpdateTime,
            })
            .ToList();

        var productUrlItemList = productUrlList
            .Select(e => new ProductUrlListItem {
                Id = e.Id,
                Name = e.Name,
                Url = e.Url,
                TypeName = e.Type.Name,
                LoginUser = e.LoginUser,
                LoginPassword = e.LoginPassword,
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        var productUrlTypeList = new List<Dictionary<string, object>> {
            new Dictionary<string, object> {
                ["value"] = "",
                ["label"] = "类型查询",
            }
        };

        productUrlTypeList.AddRange(ProductUrlType.GetList()
            .Select(e => new Dictionary<string, object> {
                ["value"] = e.Id,
                ["label"] = e.Name,
            })
        );

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["productUrlList"] = productUrlItemList,
                ["productUrlCount"] = productUrlCount,
                ["productUrlTypeList"] = productUrlTypeList,
            },
        });
    }

    [HttpPost]
    [Route("/api/product-url/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamEmptyValidator, IdParam>(param);

        long productUrlId = param.Id;

        var model = new ProductUrlDetailModel {
            Id = 0,
            ProductId = 0,
            ProductName = "",
            Url = "",
            TypeCode = 0,
            LoginUser = "",
            LoginPassword = "",
        };

        if (productUrlId > 0)
        {
            var productUrl = db.ProductUrl.Find(productUrlId);
            if (productUrl is null)
            {
                throw new ValidationException($"指定的网址#{productUrlId}不存在！");
            }

            long productId = productUrl.ProductId;
            var product = db.Product.Find(productId);
            if (product is null)
            {
                throw new ValidationException($"指定的产品#{productId}不存在!");
            }

            model = new ProductUrlDetailModel {
                Id = productUrl.Id,
                ProductId = productId,
                ProductName = product.Name,
                Url = productUrl.Url,
                TypeCode = productUrl.Type.Id,
                LoginUser = productUrl.LoginUser,
                LoginPassword = productUrl.LoginPassword,
                Description = productUrl.Description,
            };
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = model,
        });
    }

    [HttpPost]
    [Route("/api/product-url/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] ProductUrlDetailModel model)
    {
        ValidationUtil.Validate<ProductUrlDetailModelValidator, ProductUrlDetailModel>(model);

        long productUrlId = model.Id;
        bool isCreate = productUrlId == 0;

        ProductUrlEntity? productUrl;
        if (isCreate == true)
        {
            productUrl = new ProductUrlEntity();
        }
        else
        {
            productUrl = db.ProductUrl.Find(productUrlId);
            if (productUrl is null)
            {
                throw new ValidationException($"指定的网址管理#{productUrlId}不存在!");
            }
        }

        productUrl.ProductId = model.ProductId;
        productUrl.Url = model.Url ?? "";
        productUrl.Type = ProductUrlType.GetById(model.TypeCode);
        productUrl.LoginUser = model.LoginUser ?? "";
        productUrl.LoginPassword = model.LoginPassword ?? "";
        productUrl.Description = model.Description ?? "";

        if (isCreate == true)
        {
            db.ProductUrl.Add(productUrl);
        }

        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        if (isCreate == true)
        {
            string actionDesc = $"#{productUrl.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductUrlCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"#{productUrl.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductUrlUpdate, actionDesc);
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/product-url/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long productUrlId = param.Id;
        var productUrl = db.ProductUrl.Find(productUrlId);
        if (productUrl is null)
        {
            throw new ValidationException($"指定的网址管理#{productUrlId}不存在!");
        }

        db.ProductUrl.Remove(productUrl);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"#{productUrl.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductUrlDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}