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
public class ProductServiceController(
    Db db,
    UserLoginService userLoginService,
    ActionLogService actionLogService) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private class ProductServiceJoinItem
    {
        public ProductServiceEntity ProductService { get; set; } = new();

        public CollegeEntity College { get; set; } = new();

        public ProductEntity Product { get; set; } = new();

        public UserEntity User { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/product-service/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductServiceListParam param)
    {
        ValidationUtil.Validate<ProductServiceListParamValidator, ProductServiceListParam>(param);

        IQueryable<ProductServiceJoinItem> query = db.ProductService
            .Join(db.College,
                ps => ps.CollegeId,
                c => c.Id,
                (ps, c) => new ProductServiceJoinItem {
                    ProductService = ps,
                    College = c,
                })
            .Join(db.Product,
                ps => ps.ProductService.ProductId,
                p => p.Id,
                (ps, p) => new ProductServiceJoinItem {
                    ProductService = ps.ProductService,
                    College = ps.College,
                    Product = p,
                })
            .Join(db.User,
                pii => pii.ProductService.UserId,
                u => u.Id,
                (pii, u) => new ProductServiceJoinItem {
                    ProductService = pii.ProductService,
                    College = pii.College,
                    Product = pii.Product,
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

        int productServiceCount = query.Count();

        // 排序字段
        Expression<Func<ProductServiceJoinItem, object>> sortField = e => e.ProductService.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(ProductServiceListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductService.CreateTime;
            }

            if (string.Compare(sortFieldParam, nameof(ProductServiceListItem.UpdateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductService.UpdateTime;
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
            .Select(e => new {
                Id = e.ProductService.Id,
                CollegeName = e.College.Name,
                ProductName = e.Product.Name,
                ProjectType = e.ProductService.ProjectType,
                ServiceDate = e.ProductService.ServiceDate,
                ServiceCount = e.ProductService.ServiceCount,
                ServiceUrl = e.ProductService.ServiceUrl,
                LoginUser = e.ProductService.LoginUser,
                LoginPassword = e.ProductService.LoginPassword,
                ContactTeacher = e.ProductService.ContactTeacher,
                ContactMobile = e.ProductService.ContactMobile,
                Description = e.ProductService.Description,
                CreateTime = e.ProductService.CreateTime,
                UpdateTime = e.ProductService.UpdateTime,
                UserName = e.User.UserName,
            })
            .ToList();

        var productServiceItemList = productServiceList
            .Select(e => new ProductServiceListItem {
                Id = e.Id,
                CollegeName = e.CollegeName,
                ProductName = e.ProductName,
                ProjectTypeName = e.ProjectType.Name,
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
                UserName = e.UserName,
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
        ValidationUtil.Validate<IdParamEmptyValidator, IdParam>(param);

        long productServiceId = param.Id;

        var model = new ProductServiceDetailModel {
            Id = 0,
            CollegeId = 0,
            ProductId = 0,
            ProjectTypeCode = 0,
            ServiceDate = DateTime.Today.ToString("yyyy-MM-dd"),
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
            var productService = db.ProductService.Find(productServiceId);
            if (productService is null)
            {
                throw new ValidationException($"指定的售后维护#{productServiceId}不存在!");
            }

            long collegeId = productService.CollegeId;
            var college = db.Find<CollegeEntity>(collegeId);
            if (college is null)
            {
                throw new ValidationException($"指定的学校#{collegeId}不存在!");
            }

            long productId = productService.ProductId;
            var product = db.Find<ProductEntity>(productId);
            if (product is null)
            {
                throw new ValidationException($"指定的产品#{productId}不存在!");
            }

            model = new ProductServiceDetailModel {
                Id = productService.Id,
                CollegeId = productService.CollegeId,
                CollegeName = college.Name,
                ProductId = productService.ProductId,
                ProductName = product.Name,
                ProjectTypeCode = productService.ProjectType.Id,
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

        long loginUserId = userLoginService.GetLoginUserId();

        long productServiceId = model.Id;
        bool isCreate = productServiceId == 0;

        ProductServiceEntity? productService;
        if (isCreate == true)
        {
            productService = new ProductServiceEntity();
        }
        else
        {
            productService = db.ProductService.Find(productServiceId);
            if (productService is null)
            {
                throw new ValidationException($"指定的售后服务#{productServiceId}不存在!");
            }
        }

        productService.CollegeId = model.CollegeId;
        productService.ProductId = model.ProductId;
        productService.ProjectType = ProjectType.GetById(model.ProjectTypeCode);
        productService.ServiceDate = DateTime.Parse(model.ServiceDate).Date;
        productService.ServiceCount = model.ServiceCount;
        productService.ServiceUrl = model.ServiceUrl ?? "";
        productService.LoginUser = model.LoginUser ?? "";
        productService.LoginPassword = model.LoginPassword ?? "";
        productService.ContactTeacher = model.ContactTeacher ?? "";
        productService.ContactMobile = model.ContactMobile ?? "";
        productService.Description = model.Description ?? "";
        productService.UserId = loginUserId;

        if (isCreate == true)
        {
            db.ProductService.Add(productService);
        }
        else
        {
            db.ProductService.Update(productService);
        }

        db.SaveChanges();

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
        var productService = db.ProductService.Find(productServiceId);
        if (productService is null)
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