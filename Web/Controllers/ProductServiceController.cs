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
using Service.ProductEval;
using Service.ProductRent;
using Service.ProductSale;
using Service.ProductService;
using Web.Models.ProductService;

namespace Web.Controllers;

[Auth(PassRes.ProductService.ProductSaleService)]
public class ProductServiceController(
    Db db,
    UserLoginService userLoginService,
    UserProvinceService userProvinceService,
    ProductServiceService productServiceService,
    ProductSaleService productSaleService,
    ProductRentService productRentService,
    ProductEvalService productEvalService,
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

        long loginUserId = userLoginService.GetLoginUserId();
        bool isAdminLogin = userLoginService.IsAdmin(loginUserId);

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
                })
            .AsNoTracking();

        if (isAdminLogin == false)
        {
            var userProvinceList = userProvinceService.GetUserProvinceList(loginUserId);
            query = query.Where(e => userProvinceList.Contains(e.College.Province));
        }

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

            if (string.Compare(sortFieldParam, nameof(ProductServiceListItem.ProjectTypeName), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductService.ProjectType;
            }

            if (string.Compare(sortFieldParam, nameof(ProductServiceListItem.ProblemTypeName), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductService.ProblemType;
            }

            if (string.Compare(sortFieldParam, nameof(ProductServiceListItem.CollegeName), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.College.Name;
            }

            if (string.Compare(sortFieldParam, nameof(ProductServiceListItem.ProductName), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.Product.Name;
            }

            if (string.Compare(sortFieldParam, nameof(ProductServiceListItem.ContactTeacher), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductService.ContactTeacher;
            }

            if (string.Compare(sortFieldParam, nameof(ProductServiceListItem.ServiceCount), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductService.ServiceCount;
            }

            // if (string.Compare(sortFieldParam, nameof(ProductServiceListItem.UpdateTime), StringComparison.OrdinalIgnoreCase) == 0)
            // {
            //     sortField = e => e.ProductService.UpdateTime;
            // }

            if (string.Compare(sortFieldParam, nameof(ProductServiceListItem.UserName), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductService.UserId;
            }

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
                ProblemType = e.ProductService.ProblemType,
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
                ProblemTypeName = e.ProblemType.Name,
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
            CollegeName = "",
            ProductId = 0,
            ProductName = "",
            ProjectTypeCode = 0,
            ProblemTypeCode = 0,
            ServiceDate = DateTime.Today.ToString("yyyy-MM-dd"),
            // ServiceCount = 0,
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
            var college = db.College.Find(collegeId);
            if (college is null)
            {
                throw new ValidationException($"指定的学校#{collegeId}不存在!");
            }

            long productId = productService.ProductId;
            var product = db.Product.Find(productId);
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
                ProblemTypeCode = productService.ProblemType.Id,
                ServiceDate = productService.ServiceDate.ToString("yyyy-MM-dd"),
                // ServiceCount = productService.ServiceCount,
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

        var projectType = ProjectType.GetById(model.ProjectTypeCode);
        var problemType = ProblemType.GetById(model.ProblemTypeCode);
        long collegeId = model.CollegeId;
        long productId = model.ProductId;

        // 检查 项目类型 学校 和 产品 关联
        if (projectType == ProjectType.Sale)
        {
            var productSale = db.ProductSale.AsNoTracking()
                .Where(e => e.CollegeId == collegeId && e.ProductId == productId)
                .Take(1)
                .SingleOrDefault();
            if (productSale is null)
            {
                throw new ValidationException("指定的学校和产品组合在销售目录中不存在！");
            }
        }

        if (projectType == ProjectType.Rent)
        {
            var productRent = db.ProductRent.AsNoTracking()
                .Where(e => e.CollegeId == collegeId && e.ProductId == productId)
                .Take(1)
                .SingleOrDefault();
            if (productRent is null)
            {
                throw new ValidationException("指定的学校和产品组合在租赁目录中不存在！");
            }

            if (productRent.EndDate < DateTime.Today)
            {
                throw new ValidationException("指定的学校和产品组合在租赁目录中已过期！");
            }
        }

        if (projectType == ProjectType.Eval)
        {
            var productEval = db.ProductEval.AsNoTracking()
                .Where(e => e.CollegeId == collegeId && e.ProductId == productId)
                .Take(1)
                .SingleOrDefault();
            if (productEval is null)
            {
                throw new ValidationException("指定的学校和产品组合在试用目录中不存在！");
            }

            if (productEval.EndDate < DateTime.Today)
            {
                throw new ValidationException("指定的学校和产品组合在试用目录中已过期！");
            }
        }

        productService.CollegeId = collegeId;
        productService.ProductId = productId;
        productService.ProjectType = projectType;
        productService.ProblemType = problemType;
        productService.ServiceDate = DateTime.Parse(model.ServiceDate).Date;
        // productService.ServiceCount = model.ServiceCount; 这个值不是从界面上取得后保存的
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

        productServiceService.RefreshServiceCount(collegeId, productId);

        productServiceId = productService.Id;

        productSaleService.UpdateProductSaleStatisticsDataByProductService(productServiceId);
        productRentService.UpdateProductRentStatisticsDataByProductService(productServiceId);
        productEvalService.UpdateProductEvalStatisticsDataByProductService(productServiceId);

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

        db.ProductService.Remove(productService);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"#{productService.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductServiceDelete, actionDesc);

        long collegeId = productService.CollegeId;
        long productId = productService.ProductId;
        productServiceService.RefreshServiceCount(collegeId, productId);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}