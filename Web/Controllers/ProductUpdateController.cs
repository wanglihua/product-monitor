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

    [HttpPost]
    [Route("/api/product-update/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductUpdateListParam param)
    {
        IQueryable<ProductUpdateEntity> query = db.Set<ProductUpdateEntity>();

        // if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        // {
        //     string nameQuery = param.NameQuery.Trim();
        //     query = query.Where(p => p.Name.Contains(nameQuery));
        // }

        int productUpdateCount = query.Count();

        // 排序字段
        Expression<Func<ProductUpdateEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();

            if (sortFieldParam == nameof(ProductUpdateListItem.CreateTime).ToUpper())
            {
                sortField = e => e.CreateTime;
            }

            if (sortFieldParam == nameof(ProductUpdateListItem.UpdateTime).ToUpper())
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

        var productUpdateList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var productUpdateItemList = productUpdateList
            .Select(e => new ProductUpdateListItem {
                Id = e.Id,
                CollegeId = e.CollegeId,
                ProductId = e.ProductId,
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
            UpdateDate = "",
            SalesManager = "",
            ContactTeacher = "",
            ContactMobile = "",
            Description = "",
        };

        if (productUpdateId > 0)
        {
            var productUpdate = db.Set<ProductUpdateEntity>().Find(productUpdateId);
            if (productUpdate != null)
            {
                model = new ProductUpdateDetailModel {
                    Id = productUpdate.Id,
                    CollegeId = productUpdate.CollegeId,
                    ProductId = productUpdate.ProductId,
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

        if (isCreate == true)
        {
            db.Add<ProductUpdateEntity>(productUpdate);
        }
        else
        {
            db.Update<ProductUpdateEntity>(productUpdate);
        }

        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
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
