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
using Service.ProductInstall;
using Web.Models.ProductInstall;

namespace Web.Controllers;

[Auth(PassRes.ProductService.ProductInstall)]
public class ProductInstallController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly ActionLogService actionLogService;
    private readonly ProductInstallExcelService productInstallExcelService;

    public ProductInstallController(
        Db db,
        UserLoginService userLoginService,
        ActionLogService actionLogService,
        ProductInstallExcelService productInstallExcelService
    )
    {
        this.db = db;
        this.userLoginService = userLoginService;
        this.actionLogService = actionLogService;
        this.productInstallExcelService = productInstallExcelService;
    }

    private class ProductInstallJoinItem
    {
        public ProductInstallEntity ProductInstall { get; set; } = new();

        public CollegeEntity College { get; set; } = new();

        public ProductEntity Product { get; set; } = new();

        public UserEntity User { get; set; } = new();
    }

    private IQueryable<ProductInstallJoinItem> GetListQuery(ProductInstallListParam param)
    {
        IQueryable<ProductInstallJoinItem> query = db.Set<ProductInstallEntity>()
            .Join(db.Set<CollegeEntity>(),
                pi => pi.CollegeId,
                c => c.Id,
                (pi, c) => new ProductInstallJoinItem {
                    ProductInstall = pi,
                    College = c,
                })
            .Join(db.Set<ProductEntity>(),
                pi => pi.ProductInstall.ProductId,
                p => p.Id,
                (pi, p) => new ProductInstallJoinItem {
                    ProductInstall = pi.ProductInstall,
                    College = pi.College,
                    Product = p,
                })
            .Join(db.Set<UserEntity>(),
                pi => pi.ProductInstall.UserId,
                u => u.Id,
                (pi, u) => new ProductInstallJoinItem {
                    ProductInstall = pi.ProductInstall,
                    College = pi.College,
                    Product = pi.Product,
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

        return query;
    }

    [HttpPost]
    [Route("/api/product-install/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductInstallListParam param)
    {
        ValidationUtil.Validate<ProductInstallListParamValidator, ProductInstallListParam>(param);

        IQueryable<ProductInstallJoinItem> query = GetListQuery(param);

        int productInstallCount = query.Count();

        // 排序字段
        Expression<Func<ProductInstallJoinItem, object>> sortField = e => e.ProductInstall.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(ProductInstallListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductInstall.CreateTime;
            }

            if (string.Compare(sortFieldParam, nameof(ProductInstallListItem.UpdateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductInstall.UpdateTime;
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

        var productInstallList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var productInstallItemList = productInstallList
            .Select(e => new ProductInstallListItem {
                Id = e.ProductInstall.Id,
                CollegeName = e.College.Name,
                ProductName = e.Product.Name,
                VersionSale = e.ProductInstall.VersionSale,
                VersionInstall = e.ProductInstall.VersionInstall,
                InstallDate = e.ProductInstall.InstallDate.ToString("yyyy-MM-dd"),
                UpdateUrl = e.ProductInstall.UpdateUrl,
                LoginUser = e.ProductInstall.LoginUser,
                LoginPassword = e.ProductInstall.LoginPassword,
                ContactTeacher = e.ProductInstall.ContactTeacher,
                ContactMobile = e.ProductInstall.ContactMobile,
                Description = e.ProductInstall.Description,
                UserName = e.User.UserName,
                CreateTime = e.ProductInstall.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.ProductInstall.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["productInstallList"] = productInstallItemList,
                ["productInstallCount"] = productInstallCount,
            },
        });
    }

    [HttpPost]
    [Route("/api/product-install/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamEmptyValidator, IdParam>(param);

        long productInstallId = param.Id;

        var model = new ProductInstallDetailModel {
            Id = 0,
            CollegeId = 0,
            CollegeName = "",
            ProductId = 0,
            ProductName = "",
            VersionSale = "",
            VersionInstall = "",
            InstallDate = DateTime.Today.ToString("yyyy-MM-dd"),
            UpdateUrl = "",
            LoginUser = "",
            LoginPassword = "",
            ContactTeacher = "",
            ContactMobile = "",
            Description = "",
        };

        if (productInstallId > 0)
        {
            var productInstall = db.Set<ProductInstallEntity>().Find(productInstallId);
            if (productInstall is null)
            {
                throw new ValidationException($"指定的产品安装#{productInstallId}不存在!");
            }

            long collegeId = productInstall.CollegeId;
            var college = db.Find<CollegeEntity>(collegeId);
            if (college is null)
            {
                throw new ValidationException($"指定的学校#{collegeId}不存在!");
            }

            long productId = productInstall.ProductId;
            var product = db.Find<ProductEntity>(productId);
            if (product is null)
            {
                throw new ValidationException($"指定的产品#{productId}不存在!");
            }

            model = new ProductInstallDetailModel {
                Id = productInstall.Id,
                CollegeId = productInstall.CollegeId,
                CollegeName = college.Name,
                ProductId = productInstall.ProductId,
                ProductName = product.Name,
                VersionSale = productInstall.VersionSale,
                VersionInstall = productInstall.VersionInstall,
                InstallDate = productInstall.InstallDate.ToString("yyyy-MM-dd"),
                UpdateUrl = productInstall.UpdateUrl,
                LoginUser = productInstall.LoginUser,
                LoginPassword = productInstall.LoginPassword,
                ContactTeacher = productInstall.ContactTeacher,
                ContactMobile = productInstall.ContactMobile,
                Description = productInstall.Description,
            };
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = model,
        });
    }

    [HttpPost]
    [Route("/api/product-install/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] ProductInstallDetailModel model)
    {
        ValidationUtil.Validate<ProductInstallDetailModelValidator, ProductInstallDetailModel>(model);

        long loginUserId = userLoginService.GetLoginUserId();

        long productInstallId = model.Id;
        bool isCreate = productInstallId == 0;

        ProductInstallEntity? productInstall;
        if (isCreate == true)
        {
            productInstall = new ProductInstallEntity();
        }
        else
        {
            productInstall = db.Set<ProductInstallEntity>().Find(productInstallId);
            if (productInstall is null)
            {
                throw new ValidationException($"指定的产品安装#{productInstallId}不存在!");
            }
        }

        productInstall.CollegeId = model.CollegeId;
        productInstall.ProductId = model.ProductId;
        productInstall.VersionSale = model.VersionSale ?? "";
        productInstall.VersionInstall = model.VersionInstall ?? "";
        productInstall.InstallDate = DateTime.Parse(model.InstallDate).Date;
        productInstall.UpdateUrl = model.UpdateUrl ?? "";
        productInstall.LoginUser = model.LoginUser ?? "";
        productInstall.LoginPassword = model.LoginPassword ?? "";
        productInstall.ContactTeacher = model.ContactTeacher ?? "";
        productInstall.ContactMobile = model.ContactMobile ?? "";
        productInstall.Description = model.Description ?? "";
        productInstall.UserId = loginUserId;

        if (isCreate == true)
        {
            db.Add<ProductInstallEntity>(productInstall);
        }
        else
        {
            db.Update<ProductInstallEntity>(productInstall);
        }

        db.SaveChanges();

        if (isCreate == true)
        {
            string actionDesc = $"#{productInstall.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductInstallCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"#{productInstall.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductInstallUpdate, actionDesc);
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/product-install/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long productInstallId = param.Id;
        var productInstall = db.Set<ProductInstallEntity>().Find(productInstallId);
        if (productInstall is null)
        {
            throw new ValidationException($"指定的产品安装#{productInstallId}不存在!");
        }

        db.Remove<ProductInstallEntity>(productInstall);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"#{productInstall.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductInstallDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }

    [HttpPost]
    [Route("/api/product-install/export")]
    [Transactional(false)]
    public IActionResult Export(ProductInstallListParam param)
    {
        ValidationUtil.Validate<ProductInstallListParamValidator, ProductInstallListParam>(param);

        IQueryable<ProductInstallJoinItem> query = GetListQuery(param);

        var productInstallList = query
            .OrderBy(e => e.ProductInstall.Id)
            .ToList();

        var productInstallItemList = productInstallList
            .Select(e => new ProductInstallExcelExportListItem {
                Id = e.ProductInstall.Id,
                CollegeName = e.College.Name,
                ProductName = e.Product.Name,
                VersionSale = e.ProductInstall.VersionSale,
                VersionInstall = e.ProductInstall.VersionInstall,
                InstallDate = e.ProductInstall.InstallDate.ToString("yyyy-MM-dd"),
                UpdateUrl = e.ProductInstall.UpdateUrl,
                LoginUser = e.ProductInstall.LoginUser,
                LoginPassword = e.ProductInstall.LoginPassword,
                ContactTeacher = e.ProductInstall.ContactTeacher,
                ContactMobile = e.ProductInstall.ContactMobile,
                Description = e.ProductInstall.Description,
                UserName = $"{e.User.LoginName}-{e.User.UserName}",
                CreateTime = e.ProductInstall.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.ProductInstall.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        var excelContent = productInstallExcelService.GetExcelContent(productInstallItemList);

        return File(excelContent, ContentType.Xlsx, "产品安装.xlsx");
    }
}