using System.Linq.Expressions;
using Core;
using Core.Auth;
using Core.Database;
using Core.Util;
using Core.Web;
using Domain;
using Domain.Dto;
using Domain.PassRes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using Service;
using Service.ProductEval;
using Service.ProductRent;
using Service.ProductSale;
using Service.ProductSpecialService;
using Web.Models.ProductSpecialService;

namespace Web.Controllers;

[Auth(PassRes.ProductService.ProductSpecialService)]
public class ProductSpecialServiceController(
    Db db,
    ProductSelectService productSelectService,
    UserLoginService userLoginService,
    UserProvinceService userProvinceService,
    ProductSpecialServiceService productSpecialServiceService,
    ProductSpecialServiceExcelService productSpecialServiceExcelService,
    ProductSaleService productSaleService,
    ProductRentService productRentService,
    ProductEvalService productEvalService,
    ActionLogService actionLogService) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private class ProductSpecialServiceJoinItem
    {
        public ProductSpecialServiceEntity ProductSpecialService { get; set; } = new();

        public CollegeEntity College { get; set; } = new();

        public ProductEntity Product { get; set; } = new();

        public UserEntity User { get; set; } = new();
    }

    [NonAction]
    private IQueryable<ProductSpecialServiceJoinItem> GetListQuery(ProductSpecialServiceListParam param)
    {
        long loginUserId = userLoginService.GetLoginUserId();
        bool isAdminLogin = userLoginService.IsAdmin(loginUserId);

        IQueryable<ProductSpecialServiceJoinItem> query = db.ProductSpecialService
            .Join(db.College,
                pss => pss.CollegeId,
                c => c.Id,
                (pss, c) => new ProductSpecialServiceJoinItem {
                    ProductSpecialService = pss,
                    College = c,
                })
            .Join(db.Product,
                pss => pss.ProductSpecialService.ProductId,
                p => p.Id,
                (pss, p) => new ProductSpecialServiceJoinItem {
                    ProductSpecialService = pss.ProductSpecialService,
                    College = pss.College,
                    Product = p,
                })
            .Join(db.User,
                pss => pss.ProductSpecialService.UserId,
                u => u.Id,
                (pss, u) => new ProductSpecialServiceJoinItem {
                    ProductSpecialService = pss.ProductSpecialService,
                    College = pss.College,
                    Product = pss.Product,
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

        if (string.IsNullOrWhiteSpace(param.CreateDateBeginQuery) == false)
        {
            string createDateBeginQuery = param.CreateDateBeginQuery.Trim();
            if (DateTime.TryParse(createDateBeginQuery, out DateTime createDateBegin) == true)
            {
                query = query.Where(e => e.ProductSpecialService.CreateTime >= createDateBegin);
            }
        }

        if (string.IsNullOrWhiteSpace(param.CreateDateEndQuery) == false)
        {
            string createDateEndQuery = param.CreateDateEndQuery.Trim();
            if (DateTime.TryParse(createDateEndQuery, out DateTime createDateEnd) == true)
            {
                createDateEnd = createDateEnd.AddDays(1);
                query = query.Where(e => e.ProductSpecialService.CreateTime < createDateEnd);
            }
        }

        return query;
    }

    [HttpPost]
    [Route("/api/product-special-service/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductSpecialServiceListParam param)
    {
        ValidationUtil.Validate<ProductSpecialServiceListParamValidator, ProductSpecialServiceListParam>(param);

        IQueryable<ProductSpecialServiceJoinItem> query = GetListQuery(param);

        int productSpecialServiceCount = query.Count();

        // 排序字段
        Expression<Func<ProductSpecialServiceJoinItem, object>> sortField = e => e.ProductSpecialService.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(ProductSpecialServiceListItem.CollegeName), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.College.Name;
            }

            if (string.Compare(sortFieldParam, nameof(ProductSpecialServiceListItem.ProductName), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.Product.Name;
            }

            if (string.Compare(sortFieldParam, nameof(ProductSpecialServiceListItem.ContactTeacher), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductSpecialService.ContactTeacher;
            }

            if (string.Compare(sortFieldParam, nameof(ProductSpecialServiceListItem.ServiceCount), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductSpecialService.ServiceCount;
            }

            if (string.Compare(sortFieldParam, nameof(ProductSpecialServiceListItem.UserName), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductSpecialService.UserId;
            }

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
                ProjectType = e.ProductSpecialService.ProjectType,
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
                ProjectTypeName = e.ProjectType.Name,
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
            ProjectTypeCode = 0,
            ServiceDate = DateTime.Today.ToString("yyyy-MM-dd"),
            // ServiceCount = 0,
            ServiceUrl = "",
            LoginUser = "",
            LoginPassword = "",
            ContactTeacher = "",
            ContactMobile = "",
            Description = "",
        };

        if (productSpecialServiceId > 0)
        {
            var productSpecialService = db.ProductSpecialService.Find(productSpecialServiceId);

            if (productSpecialService is null)
            {
                throw new ValidationException($"指定的产品安装#{productSpecialServiceId}不存在!");
            }

            long collegeId = productSpecialService.CollegeId;
            var college = db.College.Find(collegeId);
            if (college is null)
            {
                throw new ValidationException($"指定的学校#{collegeId}不存在!");
            }

            long productId = productSpecialService.ProductId;
            var product = db.Product.Find(productId);
            if (product is null)
            {
                throw new ValidationException($"指定的产品#{productId}不存在!");
            }
            
            var productItemList = productSelectService.GetProductSelectOptionList(productSpecialService.ProjectType, productSpecialService.CollegeId);

            if (productItemList.Any(e => e.Value == productId) == false)
            {
                productItemList.Add(new SelectOptionLongItem {
                    Value = productId,
                    Label = product.Name,
                });
            }

            model = new ProductSpecialServiceDetailModel {
                Id = productSpecialService.Id,
                CollegeId = productSpecialService.CollegeId,
                CollegeName = college.Name,
                ProductId = productSpecialService.ProductId,
                ProductName = product.Name,
                ProductList = productItemList,
                ProjectTypeCode = productSpecialService.ProjectType.Id,
                ServiceDate = productSpecialService.ServiceDate.ToString("yyyy-MM-dd"),
                // ServiceCount = productSpecialService.ServiceCount,
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
            productSpecialService = db.ProductSpecialService.Find(productSpecialServiceId);
            if (productSpecialService is null)
            {
                throw new ValidationException($"指定的特殊售后#{productSpecialServiceId}不存在!");
            }
        }

        var projectType = ProjectType.GetById(model.ProjectTypeCode);
        long collegeId = model.CollegeId;
        long productId = model.ProductId;

        // 这里 【不用】 检查 项目类型 学校 和 产品 关联

        /*
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
        */

        productSpecialService.CollegeId = collegeId;
        productSpecialService.ProductId = productId;
        productSpecialService.ProjectType = projectType;
        productSpecialService.ServiceDate = DateTime.Parse(model.ServiceDate).Date;
        // productSpecialService.ServiceCount = model.ServiceCount; 这个值不是从界面上取得后保存的
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

        productSpecialServiceService.RefreshServiceCount(collegeId, productId);

        productSpecialServiceId = productSpecialService.Id;

        productSaleService.UpdateProductSaleStatisticsDataByProductSpecialService(productSpecialServiceId);
        productRentService.UpdateProductRentStatisticsDataByProductSpecialService(productSpecialServiceId);
        productEvalService.UpdateProductEvalStatisticsDataByProductSpecialService(productSpecialServiceId);

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
        var productSpecialService = db.ProductSpecialService.Find(productSpecialServiceId);
        if (productSpecialService is null)
        {
            throw new ValidationException($"指定的特殊售后#{productSpecialServiceId}不存在!");
        }

        db.ProductSpecialService.Remove(productSpecialService);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"#{productSpecialService.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductSpecialServiceDelete, actionDesc);

        long collegeId = productSpecialService.CollegeId;
        long productId = productSpecialService.ProductId;
        productSpecialServiceService.RefreshServiceCount(collegeId, productId);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }

    [HttpPost]
    [Route("/api/product-special-service/export")]
    [Transactional(false)]
    public IActionResult Export(ProductSpecialServiceListParam param)
    {
        ValidationUtil.Validate<ProductSpecialServiceListParamValidator, ProductSpecialServiceListParam>(param);

        IQueryable<ProductSpecialServiceJoinItem> query = GetListQuery(param);

        var productSpecialServiceList = query
            .OrderBy(e => e.ProductSpecialService.Id)
            .Select(e => new {
                Id = e.ProductSpecialService.Id,
                CollegeName = e.College.Name,
                ProductName = e.Product.Name,
                ProjectType = e.ProductSpecialService.ProjectType,
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
            .Select(e => new ProductSpecialServiceExcelExportListItem {
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
                UserName = e.UserName,
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        var excelContent = productSpecialServiceExcelService.Export(productSpecialServiceItemList);

        return File(excelContent, ContentType.Xlsx, "特殊售后.xlsx");
    }
}