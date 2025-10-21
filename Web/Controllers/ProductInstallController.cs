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
using Web.Models.ProductInstall;

namespace Web.Controllers;

[Auth(PassRes.ProductService.ProductInstall)]
public class ProductInstallController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly ActionLogService actionLogService;

    public ProductInstallController(
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
    [Route("/api/product-install/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductInstallListParam param)
    {
        IQueryable<ProductInstallEntity> query = db.Set<ProductInstallEntity>();

        // if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        // {
        //     string nameQuery = param.NameQuery.Trim();
        //     query = query.Where(p => p.Name.Contains(nameQuery));
        // }

        int productInstallCount = query.Count();

        // 排序字段
        Expression<Func<ProductInstallEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();

            if (sortFieldParam == nameof(ProductInstallListItem.CreateTime).ToUpper())
            {
                sortField = e => e.CreateTime;
            }

            if (sortFieldParam == nameof(ProductInstallListItem.UpdateTime).ToUpper())
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

        var productInstallList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var productInstallItemList = productInstallList
            .Select(e => new ProductInstallListItem {
                Id = e.Id,
                CollegeId = e.CollegeId,
                ProductId = e.ProductId,
                VersionSale = e.VersionSale,
                VersionInstall = e.VersionInstall,
                InstallDate = e.InstallDate.ToString("yyyy-MM-dd"),
                UpdateUrl = e.UpdateUrl,
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
        long productInstallId = param.Id;

        var model = new ProductInstallDetailModel {
            Id = 0,
            CollegeId = 0,
            ProductId = 0,
            VersionSale = "",
            VersionInstall = "",
            InstallDate = "",
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
            if (productInstall != null)
            {
                model = new ProductInstallDetailModel {
                    Id = productInstall.Id,
                    CollegeId = productInstall.CollegeId,
                    ProductId = productInstall.ProductId,
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
            if (productInstall == null)
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

        if (isCreate == true)
        {
            db.Add<ProductInstallEntity>(productInstall);
        }
        else
        {
            db.Update<ProductInstallEntity>(productInstall);
        }

        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
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
        if (productInstall == null)
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
}
