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
using Service.ProductUpdate;
using Web.Models.ProductUpdate;

namespace Web.Controllers;

[Auth(PassRes.ProductService.ProductUpdate)]
public class ProductUpdateController(
    Db db,
    UserLoginService userLoginService,
    ProductUpdateExcelService productUpdateExcelService,
    ActionLogService actionLogService) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private class ProductUpdateJoinItem
    {
        public ProductUpdateEntity ProductUpdate { get; set; } = new();

        public CollegeEntity College { get; set; } = new();

        public ProductEntity Product { get; set; } = new();

        public UserEntity User { get; set; } = new();
    }

    [NonAction]
    private IQueryable<ProductUpdateJoinItem> GetListQuery(ProductUpdateListParam param)
    {
        IQueryable<ProductUpdateJoinItem> query = db.ProductUpdate
            .Join(db.College,
                pu => pu.CollegeId,
                c => c.Id,
                (pu, c) => new ProductUpdateJoinItem {
                    ProductUpdate = pu,
                    College = c,
                })
            .Join(db.Product,
                pu => pu.ProductUpdate.ProductId,
                p => p.Id,
                (pu, p) => new ProductUpdateJoinItem {
                    ProductUpdate = pu.ProductUpdate,
                    College = pu.College,
                    Product = p,
                })
            .Join(db.User,
                pu => pu.ProductUpdate.UserId,
                u => u.Id,
                (pu, u) => new ProductUpdateJoinItem {
                    ProductUpdate = pu.ProductUpdate,
                    College = pu.College,
                    Product = pu.Product,
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
    [Route("/api/product-update/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductUpdateListParam param)
    {
        ValidationUtil.Validate<ProductUpdateListParamValidator, ProductUpdateListParam>(param);

        IQueryable<ProductUpdateJoinItem> query = GetListQuery(param);

        int productUpdateCount = query.Count();

        // 排序字段
        Expression<Func<ProductUpdateJoinItem, object>> sortField = e => e.ProductUpdate.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(ProductUpdateListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductUpdate.CreateTime;
            }

            if (string.Compare(sortFieldParam, nameof(ProductUpdateListItem.UpdateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductUpdate.UpdateTime;
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

        var productUpdateList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .Select(e => new {
                Id = e.ProductUpdate.Id,
                CollegeName = e.College.Name,
                ProductName = e.Product.Name,
                VersionSale = e.ProductUpdate.VersionSale,
                VersionUpdate = e.ProductUpdate.VersionUpdate,
                UpdateCount = e.ProductUpdate.UpdateCount,
                UpdateUrl = e.ProductUpdate.UpdateUrl,
                LoginUser = e.ProductUpdate.LoginUser,
                LoginPassword = e.ProductUpdate.LoginPassword,
                UpdateDate = e.ProductUpdate.UpdateDate,
                SalesManager = e.ProductUpdate.SalesManager,
                ContactTeacher = e.ProductUpdate.ContactTeacher,
                ContactMobile = e.ProductUpdate.ContactMobile,
                Description = e.ProductUpdate.Description,
                UserName = e.User.UserName,
                CreateTime = e.ProductUpdate.CreateTime,
                UpdateTime = e.ProductUpdate.UpdateTime,
            })
            .ToList();

        var productUpdateItemList = productUpdateList
            .Select(e => new ProductUpdateListItem {
                Id = e.Id,
                CollegeName = e.CollegeName,
                ProductName = e.ProductName,
                VersionSale = e.VersionSale,
                VersionUpdate = e.VersionUpdate,
                UpdateCount = e.UpdateCount,
                UpdateUrl = e.UpdateUrl,
                LoginUser = e.LoginUser,
                LoginPassword = e.LoginPassword,
                UpdateDate = e.UpdateDate.ToString("yyyy-MM-dd"),
                SalesManager = e.SalesManager,
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
                ["productUpdateList"] = productUpdateItemList,
                ["productUpdateCount"] = productUpdateCount,
            },
        });
    }

    [HttpPost]
    [Route("/api/product-update/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamEmptyValidator, IdParam>(param);

        long productUpdateId = param.Id;

        var model = new ProductUpdateDetailModel {
            Id = 0,
            CollegeId = 0,
            ProductId = 0,
            VersionSale = "",
            VersionUpdate = "",
            UpdateCount = 0,
            UpdateUrl = "",
            LoginUser = "",
            LoginPassword = "",
            UpdateDate = DateTime.Today.ToString("yyyy-MM-dd"),
            SalesManager = "",
            ContactTeacher = "",
            ContactMobile = "",
            Description = "",
        };

        if (productUpdateId > 0)
        {
            var productUpdate = db.ProductUpdate.Find(productUpdateId);
            if (productUpdate is null)
            {
                throw new ValidationException($"指定的产品更新#{productUpdateId}不存在!");
            }

            long collegeId = productUpdate.CollegeId;
            var college = db.Find<CollegeEntity>(collegeId);
            if (college is null)
            {
                throw new ValidationException($"指定的学校#{collegeId}不存在!");
            }

            long productId = productUpdate.ProductId;
            var product = db.Find<ProductEntity>(productId);
            if (product is null)
            {
                throw new ValidationException($"指定的产品#{productId}不存在!");
            }

            model = new ProductUpdateDetailModel {
                Id = productUpdate.Id,
                CollegeId = productUpdate.CollegeId,
                CollegeName = college.Name,
                ProductId = productUpdate.ProductId,
                ProductName = product.Name,
                VersionSale = productUpdate.VersionSale,
                VersionUpdate = productUpdate.VersionUpdate,
                UpdateCount = productUpdate.UpdateCount,
                UpdateUrl = productUpdate.UpdateUrl,
                LoginUser = productUpdate.LoginUser,
                LoginPassword = productUpdate.LoginPassword,
                UpdateDate = productUpdate.UpdateDate.ToString("yyyy-MM-dd"),
                SalesManager = productUpdate.SalesManager,
                ContactTeacher = productUpdate.ContactTeacher,
                ContactMobile = productUpdate.ContactMobile,
                Description = productUpdate.Description,
            };
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = model,
        });
    }

    [HttpPost]
    [Route("/api/product-update/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] ProductUpdateDetailModel model)
    {
        ValidationUtil.Validate<ProductUpdateDetailModelValidator, ProductUpdateDetailModel>(model);

        long loginUserId = userLoginService.GetLoginUserId();

        long productUpdateId = model.Id;
        bool isCreate = productUpdateId == 0;

        ProductUpdateEntity? productUpdate;
        if (isCreate == true)
        {
            productUpdate = new ProductUpdateEntity();
        }
        else
        {
            productUpdate = db.ProductUpdate.Find(productUpdateId);
            if (productUpdate is null)
            {
                throw new ValidationException($"指定的产品更新#{productUpdateId}不存在!");
            }
        }

        productUpdate.CollegeId = model.CollegeId;
        productUpdate.ProductId = model.ProductId;
        productUpdate.VersionSale = model.VersionSale ?? "";
        productUpdate.VersionUpdate = model.VersionUpdate ?? "";
        productUpdate.UpdateCount = model.UpdateCount;
        productUpdate.UpdateUrl = model.UpdateUrl ?? "";
        productUpdate.LoginUser = model.LoginUser ?? "";
        productUpdate.LoginPassword = model.LoginPassword ?? "";
        productUpdate.UpdateDate = DateTime.Parse(model.UpdateDate).Date;
        productUpdate.SalesManager = model.SalesManager ?? "";
        productUpdate.ContactTeacher = model.ContactTeacher ?? "";
        productUpdate.ContactMobile = model.ContactMobile ?? "";
        productUpdate.Description = model.Description ?? "";
        productUpdate.UserId = loginUserId;

        if (isCreate == true)
        {
            db.ProductUpdate.Add(productUpdate);
        }
        else
        {
            db.ProductUpdate.Update(productUpdate);
        }

        db.SaveChanges();


        if (isCreate == true)
        {
            string actionDesc = $"#{productUpdate.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductUpdateCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"#{productUpdate.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductUpdateUpdate, actionDesc);
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/product-update/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long productUpdateId = param.Id;
        var productUpdate = db.ProductUpdate.Find(productUpdateId);
        if (productUpdate is null)
        {
            throw new ValidationException($"指定的产品更新#{productUpdateId}不存在!");
        }

        db.Remove<ProductUpdateEntity>(productUpdate);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"#{productUpdate.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductUpdateDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }

    [HttpPost]
    [Route("/api/product-update/export")]
    [Transactional(false)]
    public IActionResult Export(ProductUpdateListParam param)
    {
        ValidationUtil.Validate<ProductUpdateListParamValidator, ProductUpdateListParam>(param);

        IQueryable<ProductUpdateJoinItem> query = GetListQuery(param);

        var productUpdateList = query
            .OrderBy(e => e.ProductUpdate.Id)
            .Select(e => new {
                Id = e.ProductUpdate.Id,
                CollegeName = e.College.Name,
                ProductName = e.Product.Name,
                VersionSale = e.ProductUpdate.VersionSale,
                VersionUpdate = e.ProductUpdate.VersionUpdate,
                UpdateCount = e.ProductUpdate.UpdateCount,
                UpdateUrl = e.ProductUpdate.UpdateUrl,
                LoginUser = e.ProductUpdate.LoginUser,
                LoginPassword = e.ProductUpdate.LoginPassword,
                UpdateDate = e.ProductUpdate.UpdateDate,
                ContactTeacher = e.ProductUpdate.ContactTeacher,
                ContactMobile = e.ProductUpdate.ContactMobile,
                LoginName = e.User.LoginName,
                UserName = e.User.UserName,
                Description = e.ProductUpdate.Description,
                CreateTime = e.ProductUpdate.CreateTime,
                UpdateTime = e.ProductUpdate.UpdateTime,
            })
            .ToList();

        var productUpdateItemList = productUpdateList
            .Select(e => new ProductUpdateExcelExportListItem {
                Id = e.Id,
                CollegeName = e.CollegeName,
                ProductName = e.ProductName,
                VersionSale = e.VersionSale,
                VersionUpdate = e.VersionUpdate,
                UpdateCount = e.UpdateCount,
                UpdateUrl = e.UpdateUrl,
                LoginUser = e.LoginUser,
                LoginPassword = e.LoginPassword,
                UpdateDate = e.UpdateDate.ToString("yyyy-MM-dd"),
                ContactTeacher = e.ContactTeacher,
                ContactMobile = e.ContactMobile,
                UserName = $"{e.LoginName}-{e.UserName}",
                Description = e.Description,
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        var excelContent = productUpdateExcelService.GetExcelContent(productUpdateItemList);

        return File(excelContent, ContentType.Xlsx, "产品更新.xlsx");
    }
}