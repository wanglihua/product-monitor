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
using Web.Models.ProductUpdate;

namespace Web.Controllers;

[Auth(PassRes.ProductService.ProductUpdate)]
public class ProductUpdateController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly ActionLogService actionLogService;

    public ProductUpdateController(
        Db db,
        UserLoginService userLoginService,
        ActionLogService actionLogService
    )
    {
        this.db = db;
        this.userLoginService = userLoginService;
        this.actionLogService = actionLogService;
    }

    private class ProductUpdateJoinItem
    {
        public ProductUpdateEntity ProductUpdate { get; set; } = new();

        public CollegeEntity College { get; set; } = new();

        public ProductEntity Product { get; set; } = new();

        public UserEntity User { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/product-update/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductUpdateListParam param)
    {
        IQueryable<ProductUpdateJoinItem> query = db.Set<ProductUpdateEntity>()
            .Join(db.Set<CollegeEntity>(),
                pu => pu.CollegeId,
                c => c.Id,
                (pu, c) => new ProductUpdateJoinItem {
                    ProductUpdate = pu,
                    College = c,
                })
            .Join(db.Set<ProductEntity>(),
                pii => pii.ProductUpdate.ProductId,
                p => p.Id,
                (pii, p) => new ProductUpdateJoinItem {
                    ProductUpdate = pii.ProductUpdate,
                    College = pii.College,
                    Product = p,
                })
            .Join(db.Set<UserEntity>(),
                pii => pii.ProductUpdate.UserId,
                u => u.Id,
                (pii, u) => new ProductUpdateJoinItem {
                    ProductUpdate = pii.ProductUpdate,
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

        int productUpdateCount = query.Count();

        // 排序字段
        Expression<Func<ProductUpdateJoinItem, object>> sortField = e => e.ProductUpdate.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();

            if (sortFieldParam == nameof(ProductUpdateListItem.CreateTime).ToUpper())
            {
                sortField = e => e.ProductUpdate.CreateTime;
            }

            if (sortFieldParam == nameof(ProductUpdateListItem.UpdateTime).ToUpper())
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
            .ToList();

        var productUpdateItemList = productUpdateList
            .Select(e => new ProductUpdateListItem {
                Id = e.ProductUpdate.Id,
                CollegeName = e.College.Name,
                ProductName = e.Product.Name,
                VersionSale = e.ProductUpdate.VersionSale,
                VersionUpdate = e.ProductUpdate.VersionUpdate,
                UpdateCount = e.ProductUpdate.UpdateCount,
                UpdateUrl = e.ProductUpdate.UpdateUrl,
                LoginUser = e.ProductUpdate.LoginUser,
                LoginPassword = e.ProductUpdate.LoginPassword,
                UpdateDate = e.ProductUpdate.UpdateDate.ToString("yyyy-MM-dd"),
                SalesManager = e.ProductUpdate.SalesManager,
                ContactTeacher = e.ProductUpdate.ContactTeacher,
                ContactMobile = e.ProductUpdate.ContactMobile,
                Description = e.ProductUpdate.Description,
                UserName = e.User.UserName,
                CreateTime = e.ProductUpdate.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.ProductUpdate.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
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
            var productUpdate = db.Set<ProductUpdateEntity>().Find(productUpdateId);
            if (productUpdate == null)
            {
                throw new ValidationException($"指定的产品更新#{productUpdateId}不存在!");
            }

            long collegeId = productUpdate.CollegeId;
            var college = db.Find<CollegeEntity>(collegeId);
            if (college == null)
            {
                throw new ValidationException($"指定的学校#{collegeId}不存在!");
            }

            long productId = productUpdate.ProductId;
            var product = db.Find<ProductEntity>(productId);
            if (product == null)
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
            productUpdate = db.Set<ProductUpdateEntity>().Find(productUpdateId);
            if (productUpdate == null)
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
            db.Add<ProductUpdateEntity>(productUpdate);
        }
        else
        {
            db.Update<ProductUpdateEntity>(productUpdate);
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
        var productUpdate = db.Set<ProductUpdateEntity>().Find(productUpdateId);
        if (productUpdate == null)
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
}