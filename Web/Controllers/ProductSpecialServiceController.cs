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
using Web.Models.ProductSpecialService;

namespace Web.Controllers;

[Auth(PassRes.ProductService.ProductSpecialService)]
public class ProductSpecialServiceController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly ActionLogService actionLogService;

    public ProductSpecialServiceController(
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
    [Route("/api/product-special-service/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductSpecialServiceListParam param)
    {
        IQueryable<ProductSpecialServiceEntity> query = db.Set<ProductSpecialServiceEntity>();

        // if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        // {
        //     string nameQuery = param.NameQuery.Trim();
        //     query = query.Where(p => p.Name.Contains(nameQuery));
        // }

        int productSpecialServiceCount = query.Count();

        // 排序字段
        Expression<Func<ProductSpecialServiceEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();

            if (sortFieldParam == nameof(ProductSpecialServiceListItem.CreateTime).ToUpper())
            {
                sortField = e => e.CreateTime;
            }

            if (sortFieldParam == nameof(ProductSpecialServiceListItem.UpdateTime).ToUpper())
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

        var productSpecialServiceList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var productSpecialServiceItemList = productSpecialServiceList
            .Select(e => new ProductSpecialServiceListItem {
                Id = e.Id,
                CollegeId = e.CollegeId,
                ProductId = e.ProductId,
                ServiceDate = e.ServiceDate.ToString("yyyy-MM-dd"),
                ServiceCount = e.ServiceCount,
                ServiceUrl = e.ServiceUrl,
                LoginUser = e.LoginUser,
                LoginPassword = e.LoginPassword,
                ContactTeacher = e.ContactTeacher,
                ContactMobile = e.ContactMobile,
                Description = e.Description,
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["productSpecialServiceList"] = productSpecialServiceItemList,
                ["productSpecialServiceCount"] = productSpecialServiceCount,
            },
        });
    }

    [HttpPost]
    [Route("/api/product-special-service/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        long productSpecialServiceId = param.Id;

        var model = new ProductSpecialServiceDetailModel {
            Id = 0,
            CollegeId = 0,
            ProductId = 0,
            ServiceDate = "",
            ServiceCount = 0,
            ServiceUrl = "",
            LoginUser = "",
            LoginPassword = "",
            ContactTeacher = "",
            ContactMobile = "",
            Description = "",
        };

        if (productSpecialServiceId > 0)
        {
            var productSpecialService = db.Set<ProductSpecialServiceEntity>().Find(productSpecialServiceId);
            if (productSpecialService != null)
            {
                model = new ProductSpecialServiceDetailModel {
                    Id = productSpecialService.Id,
                    CollegeId = productSpecialService.CollegeId,
                    ProductId = productSpecialService.ProductId,
                    ServiceDate = productSpecialService.ServiceDate.ToString("yyyy-MM-dd"),
                    ServiceCount = productSpecialService.ServiceCount,
                    ServiceUrl = productSpecialService.ServiceUrl,
                    LoginUser = productSpecialService.LoginUser,
                    LoginPassword = productSpecialService.LoginPassword,
                    ContactTeacher = productSpecialService.ContactTeacher,
                    ContactMobile = productSpecialService.ContactMobile,
                    Description = productSpecialService.Description,
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
    [Route("/api/product-special-service/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] ProductSpecialServiceDetailModel model)
    {
        ValidationUtil.Validate<ProductSpecialServiceDetailModelValidator, ProductSpecialServiceDetailModel>(model);

        long productSpecialServiceId = model.Id;
        bool isCreate = productSpecialServiceId == 0;

        ProductSpecialServiceEntity? productSpecialService;
        if (isCreate == true)
        {
            productSpecialService = new ProductSpecialServiceEntity();
        }
        else
        {
            productSpecialService = db.Set<ProductSpecialServiceEntity>().Find(productSpecialServiceId);
            if (productSpecialService == null)
            {
                throw new ValidationException($"指定的特殊售后#{productSpecialServiceId}不存在!");
            }
        }

        productSpecialService.CollegeId = model.CollegeId;
        productSpecialService.ProductId = model.ProductId;
        productSpecialService.ServiceDate = DateTime.Parse(model.ServiceDate).Date;
        productSpecialService.ServiceCount = model.ServiceCount;
        productSpecialService.ServiceUrl = model.ServiceUrl ?? "";
        productSpecialService.LoginUser = model.LoginUser ?? "";
        productSpecialService.LoginPassword = model.LoginPassword ?? "";
        productSpecialService.ContactTeacher = model.ContactTeacher ?? "";
        productSpecialService.ContactMobile = model.ContactMobile ?? "";
        productSpecialService.Description = model.Description ?? "";

        if (isCreate == true)
        {
            db.Add<ProductSpecialServiceEntity>(productSpecialService);
        }
        else
        {
            db.Update<ProductSpecialServiceEntity>(productSpecialService);
        }

        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        if (isCreate == true)
        {
            string actionDesc = $"#{productSpecialService.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductSpecialServiceCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"#{productSpecialService.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductSpecialServiceUpdate, actionDesc);
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/product-special-service/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long productSpecialServiceId = param.Id;
        var productSpecialService = db.Set<ProductSpecialServiceEntity>().Find(productSpecialServiceId);
        if (productSpecialService == null)
        {
            throw new ValidationException($"指定的特殊售后#{productSpecialServiceId}不存在!");
        }

        db.Remove<ProductSpecialServiceEntity>(productSpecialService);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"#{productSpecialService.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductSpecialServiceDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}