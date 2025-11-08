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
using Web.Models.ProductUrl;

namespace Web.Controllers;

[Auth(PassRes.BasicSetting.ProductUrl)]
public class ProductUrlController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly ActionLogService actionLogService;

    public ProductUrlController(
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
    [Route("/api/product-url/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductUrlListParam param)
    {
        IQueryable<ProductUrlEntity> query = db.Set<ProductUrlEntity>();

        var typeQuery = ProductUrlType.GetById(param.TypeQuery);
        if (typeQuery != ProductUrlType.None)
        {
            query = query.Where(e => e.Type == typeQuery);
        }

        if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        {
            string nameQuery = param.NameQuery.Trim();
            query = query.Where(p => p.Name.Contains(nameQuery));
        }

        int productUrlCount = query.Count();

        // 排序字段
        Expression<Func<ProductUrlEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();

            if (sortFieldParam == nameof(ProductUrlListItem.CreateTime).ToUpper())
            {
                sortField = e => e.CreateTime;
            }

            if (sortFieldParam == nameof(ProductUrlListItem.UpdateTime).ToUpper())
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

        var productUrlList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
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
        long productUrlId = param.Id;

        var model = new ProductUrlDetailModel {
            Id = 0,
            Name = "",
            Url = "",
            TypeCode = 0,
            LoginUser = "",
            LoginPassword = "",
        };

        if (productUrlId > 0)
        {
            var productUrl = db.Set<ProductUrlEntity>().Find(productUrlId);
            if (productUrl != null)
            {
                model = new ProductUrlDetailModel {
                    Id = productUrl.Id,
                    Name = productUrl.Name,
                    Url = productUrl.Url,
                    TypeCode = productUrl.Type.Id,
                    LoginUser = productUrl.LoginUser,
                    LoginPassword = productUrl.LoginPassword,
                    Description = productUrl.Description,
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
            productUrl = db.Set<ProductUrlEntity>().Find(productUrlId);
            if (productUrl == null)
            {
                throw new ValidationException($"指定的网址管理#{productUrlId}不存在!");
            }
        }

        productUrl.Name = model.Name ?? "";
        productUrl.Url = model.Url ?? "";
        productUrl.Type = ProductUrlType.GetById(model.TypeCode);
        productUrl.LoginUser = model.LoginUser ?? "";
        productUrl.LoginPassword = model.LoginPassword ?? "";
        productUrl.Description = model.Description ?? "";

        if (isCreate == true)
        {
            db.Add<ProductUrlEntity>(productUrl);
        }
        else
        {
            db.Update<ProductUrlEntity>(productUrl);
        }

        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        if (isCreate == true)
        {
            string actionDesc = $"{productUrl.Name}#{productUrl.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductUrlCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"{productUrl.Name}#{productUrl.Id}";
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
        var productUrl = db.Set<ProductUrlEntity>().Find(productUrlId);
        if (productUrl == null)
        {
            throw new ValidationException($"指定的网址管理#{productUrlId}不存在!");
        }

        db.Remove<ProductUrlEntity>(productUrl);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"{productUrl.Name}#{productUrl.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductUrlDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}