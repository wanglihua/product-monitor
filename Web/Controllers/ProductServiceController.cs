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
using Web.Models.ProductService;

namespace Web.Controllers;

[Auth(PassRes.ProductService.ProductSaleService)]
public class ProductServiceController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly ActionLogService actionLogService;

    public ProductServiceController(
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
    [Route("/api/product-service/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductServiceListParam param)
    {
        IQueryable<ProductServiceEntity> query = db.Set<ProductServiceEntity>();

        // if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        // {
        //     string nameQuery = param.NameQuery.Trim();
        //     query = query.Where(p => p.Name.Contains(nameQuery));
        // }

        int productServiceCount = query.Count();

        // 排序字段
        Expression<Func<ProductServiceEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();

            if (sortFieldParam == nameof(ProductServiceListItem.CreateTime).ToUpper())
            {
                sortField = e => e.CreateTime;
            }

            if (sortFieldParam == nameof(ProductServiceListItem.UpdateTime).ToUpper())
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

        var productServiceList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var productServiceItemList = productServiceList
            .Select(e => new ProductServiceListItem {
                Id = e.Id,
                CollegeId = e.CollegeId,
                ProductId = e.ProductId,
                ProjectTypeCode = e.ProjectTypeCode,
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
                ["productServiceList"] = productServiceItemList,
                ["productServiceCount"] = productServiceCount,
            },
        });
    }

    [HttpPost]
    [Route("/api/product-service/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        long productServiceId = param.Id;

        var model = new ProductServiceDetailModel {
            Id = 0,
            CollegeId = 0,
            ProductId = 0,
            ProjectTypeCode = 0,
            ServiceDate = "",
            ServiceCount = 0,
            ServiceUrl = "",
            LoginUser = "",
            LoginPassword = "",
            ContactTeacher = "",
            ContactMobile = "",
            Description = "",
        };

        if (productServiceId > 0)
        {
            var productService = db.Set<ProductServiceEntity>().Find(productServiceId);
            if (productService != null)
            {
                model = new ProductServiceDetailModel {
                    Id = productService.Id,
                    CollegeId = productService.CollegeId,
                    ProductId = productService.ProductId,
                    ProjectTypeCode = productService.ProjectTypeCode,
                    ServiceDate = productService.ServiceDate.ToString("yyyy-MM-dd"),
                    ServiceCount = productService.ServiceCount,
                    ServiceUrl = productService.ServiceUrl,
                    LoginUser = productService.LoginUser,
                    LoginPassword = productService.LoginPassword,
                    ContactTeacher = productService.ContactTeacher,
                    ContactMobile = productService.ContactMobile,
                    Description = productService.Description,
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
    [Route("/api/product-service/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] ProductServiceDetailModel model)
    {
        ValidationUtil.Validate<ProductServiceDetailModelValidator, ProductServiceDetailModel>(model);

        long productServiceId = model.Id;
        bool isCreate = productServiceId == 0;

        ProductServiceEntity? productService;
        if (isCreate == true)
        {
            productService = new ProductServiceEntity();
        }
        else
        {
            productService = db.Set<ProductServiceEntity>().Find(productServiceId);
            if (productService == null)
            {
                throw new ValidationException($"指定的售后服务#{productServiceId}不存在!");
            }
        }

        productService.CollegeId = model.CollegeId;
        productService.ProductId = model.ProductId;
        productService.ProjectTypeCode = model.ProjectTypeCode;
        productService.ServiceDate = DateTime.Parse(model.ServiceDate).Date;
        productService.ServiceCount = model.ServiceCount;
        productService.ServiceUrl = model.ServiceUrl ?? "";
        productService.LoginUser = model.LoginUser ?? "";
        productService.LoginPassword = model.LoginPassword ?? "";
        productService.ContactTeacher = model.ContactTeacher ?? "";
        productService.ContactMobile = model.ContactMobile ?? "";
        productService.Description = model.Description ?? "";

        if (isCreate == true)
        {
            db.Add<ProductServiceEntity>(productService);
        }
        else
        {
            db.Update<ProductServiceEntity>(productService);
        }

        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        if (isCreate == true)
        {
            string actionDesc = $"#{productService.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductServiceCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"#{productService.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductServiceUpdate, actionDesc);
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/product-service/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long productServiceId = param.Id;
        var productService = db.Set<ProductServiceEntity>().Find(productServiceId);
        if (productService == null)
        {
            throw new ValidationException($"指定的售后服务#{productServiceId}不存在!");
        }

        db.Remove<ProductServiceEntity>(productService);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"#{productService.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductServiceDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}
