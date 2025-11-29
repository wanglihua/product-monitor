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

    private class ProductSpecialServiceJoinItem
    {
        public ProductSpecialServiceEntity ProductSpecialService { get; set; } = new();

        public CollegeEntity College { get; set; } = new();

        public ProductEntity Product { get; set; } = new();

        public UserEntity User { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/product-special-service/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductSpecialServiceListParam param)
    {
        ValidationUtil.Validate<ProductSpecialServiceListParamValidator, ProductSpecialServiceListParam>(param);

        IQueryable<ProductSpecialServiceJoinItem> query = db.Set<ProductSpecialServiceEntity>()
            .Join(db.Set<CollegeEntity>(),
                pss => pss.CollegeId,
                c => c.Id,
                (pss, c) => new ProductSpecialServiceJoinItem {
                    ProductSpecialService = pss,
                    College = c,
                })
            .Join(db.Set<ProductEntity>(),
                pss => pss.ProductSpecialService.ProductId,
                p => p.Id,
                (pss, p) => new ProductSpecialServiceJoinItem {
                    ProductSpecialService = pss.ProductSpecialService,
                    College = pss.College,
                    Product = p,
                })
            .Join(db.Set<UserEntity>(),
                pss => pss.ProductSpecialService.UserId,
                u => u.Id,
                (pss, u) => new ProductSpecialServiceJoinItem {
                    ProductSpecialService = pss.ProductSpecialService,
                    College = pss.College,
                    Product = pss.Product,
                    User = u,
                });

        if (string.IsNullOrWhiteSpace(param.CollegeNameQuery) == false)
        {
            string collegeNameQuery = param.CollegeNameQuery.Trim();
            query = query.Where(e => e.College.Name.Contains(collegeNameQuery));
        }

        if (string.IsNullOrWhiteSpace(param.ProductNameQuery) == false)
        {
            string productNameQuery = param.ProductNameQuery.Trim();
            query = query.Where(e => e.Product.Name.Contains(productNameQuery));
        }

        int productSpecialServiceCount = query.Count();

        // 排序字段
        Expression<Func<ProductSpecialServiceJoinItem, object>> sortField = e => e.ProductSpecialService.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(ProductSpecialServiceListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductSpecialService.CreateTime;
            }

            if (string.Compare(sortFieldParam, nameof(ProductSpecialServiceListItem.UpdateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductSpecialService.UpdateTime;
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
            .Select(e => new {
                Id = e.ProductSpecialService.Id,
                CollegeName = e.College.Name,
                ProductName = e.Product.Name,
                ServiceDate = e.ProductSpecialService.ServiceDate,
                ServiceCount = e.ProductSpecialService.ServiceCount,
                ServiceUrl = e.ProductSpecialService.ServiceUrl,
                LoginUser = e.ProductSpecialService.LoginUser,
                LoginPassword = e.ProductSpecialService.LoginPassword,
                ContactTeacher = e.ProductSpecialService.ContactTeacher,
                ContactMobile = e.ProductSpecialService.ContactMobile,
                Description = e.ProductSpecialService.Description,
                UserName = e.User.UserName,
                CreateTime = e.ProductSpecialService.CreateTime,
                UpdateTime = e.ProductSpecialService.UpdateTime,
            })
            .ToList();

        var productSpecialServiceItemList = productSpecialServiceList
            .Select(e => new ProductSpecialServiceListItem {
                Id = e.Id,
                CollegeName = e.CollegeName,
                ProductName = e.ProductName,
                ServiceDate = e.ServiceDate.ToString("yyyy-MM-dd"),
                ServiceCount = e.ServiceCount,
                ServiceUrl = e.ServiceUrl,
                LoginUser = e.LoginUser,
                LoginPassword = e.LoginPassword,
                ContactTeacher = e.ContactTeacher,
                ContactMobile = e.ContactMobile,
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
        ValidationUtil.Validate<IdParamEmptyValidator, IdParam>(param);

        long productSpecialServiceId = param.Id;

        var model = new ProductSpecialServiceDetailModel {
            Id = 0,
            CollegeId = 0,
            CollegeName = "",
            ProductId = 0,
            ProductName = "",
            ServiceDate = DateTime.Today.ToString("yyyy-MM-dd"),
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

            if (productSpecialService is null)
            {
                throw new ValidationException($"指定的产品安装#{productSpecialServiceId}不存在!");
            }

            long collegeId = productSpecialService.CollegeId;
            var college = db.Find<CollegeEntity>(collegeId);
            if (college is null)
            {
                throw new ValidationException($"指定的学校#{collegeId}不存在!");
            }

            long productId = productSpecialService.ProductId;
            var product = db.Find<ProductEntity>(productId);
            if (product is null)
            {
                throw new ValidationException($"指定的产品#{productId}不存在!");
            }

            model = new ProductSpecialServiceDetailModel {
                Id = productSpecialService.Id,
                CollegeId = productSpecialService.CollegeId,
                CollegeName = college.Name,
                ProductId = productSpecialService.ProductId,
                ProductName = product.Name,
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

        long loginUserId = userLoginService.GetLoginUserId();

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
            if (productSpecialService is null)
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
        productSpecialService.UserId = loginUserId;

        if (isCreate == true)
        {
            db.ProductSpecialService.Add(productSpecialService);
        }
        else
        {
            db.Update<ProductSpecialServiceEntity>(productSpecialService);
        }

        db.SaveChanges();

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
        if (productSpecialService is null)
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