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
using Web.Models.ProductRent;

namespace Web.Controllers;

[Auth(PassRes.ProjectManage.ProductRent)]
public class ProductRentController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly ActionLogService actionLogService;

    public ProductRentController(
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
    [Route("/api/product-rent/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductRentListParam param)
    {
        IQueryable<ProductRentEntity> query = db.Set<ProductRentEntity>();

        // if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        // {
        //     string nameQuery = param.NameQuery.Trim();
        //     query = query.Where(p => p.Name.Contains(nameQuery));
        // }

        int productRentCount = query.Count();

        // 排序字段
        Expression<Func<ProductRentEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();

            if (sortFieldParam == nameof(ProductRentListItem.CreateTime).ToUpper())
            {
                sortField = e => e.CreateTime;
            }

            if (sortFieldParam == nameof(ProductRentListItem.UpdateTime).ToUpper())
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

        var productRentList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var productRentItemList = productRentList
            .Select(e => new ProductRentListItem {
                Id = e.Id,
                CollegeId = e.CollegeId,
                ProductId = e.ProductId,
                RentTimes = e.RentTimes,
                ServiceTimes = e.ServiceTimes,
                TotalDays = e.TotalDays,
                RemainDays = e.RemainDays,
                BeginDate = e.BeginDate.ToString("yyyy-MM-dd"),
                EndDate = e.EndDate.ToString("yyyy-MM-dd"),
                Description = e.Description,
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["productRentList"] = productRentItemList,
                ["productRentCount"] = productRentCount,
            },
        });
    }

    [HttpPost]
    [Route("/api/product-rent/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        long productRentId = param.Id;

        var model = new ProductRentDetailModel {
            Id = 0,
            CollegeId = 0,
            ProductId = 0,
            RentTimes = 0,
            ServiceTimes = 0,
            TotalDays = 0,
            RemainDays = 0,
            BeginDate = "",
            EndDate = "",
            Description = "",
        };

        if (productRentId > 0)
        {
            var productRent = db.Set<ProductRentEntity>().Find(productRentId);
            if (productRent != null)
            {
                model = new ProductRentDetailModel {
                    Id = productRent.Id,
                    CollegeId = productRent.CollegeId,
                    ProductId = productRent.ProductId,
                    RentTimes = productRent.RentTimes,
                    ServiceTimes = productRent.ServiceTimes,
                    TotalDays = productRent.TotalDays,
                    RemainDays = productRent.RemainDays,
                    BeginDate = productRent.BeginDate.ToString("yyyy-MM-dd"),
                    EndDate = productRent.EndDate.ToString("yyyy-MM-dd"),
                    Description = productRent.Description,
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
    [Route("/api/product-rent/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] ProductRentDetailModel model)
    {
        ValidationUtil.Validate<ProductRentDetailModelValidator, ProductRentDetailModel>(model);

        long productRentId = model.Id;
        bool isCreate = productRentId == 0;

        ProductRentEntity? productRent;
        if (isCreate == true)
        {
            productRent = new ProductRentEntity();
        }
        else
        {
            productRent = db.Set<ProductRentEntity>().Find(productRentId);
            if (productRent == null)
            {
                throw new ValidationException($"指定的租赁目录#{productRentId}不存在!");
            }
        }

        productRent.CollegeId = model.CollegeId;
        productRent.ProductId = model.ProductId;
        productRent.RentTimes = model.RentTimes;
        productRent.ServiceTimes = model.ServiceTimes;
        productRent.TotalDays = model.TotalDays;
        productRent.RemainDays = model.RemainDays;
        productRent.BeginDate = DateTime.Parse(model.BeginDate).Date;
        productRent.EndDate = DateTime.Parse(model.EndDate).Date;
        productRent.Description = model.Description ?? "";

        if (isCreate == true)
        {
            db.Add<ProductRentEntity>(productRent);
        }
        else
        {
            db.Update<ProductRentEntity>(productRent);
        }

        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        if (isCreate == true)
        {
            string actionDesc = $"#{productRent.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductRentCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"#{productRent.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductRentUpdate, actionDesc);
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/product-rent/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long productRentId = param.Id;
        var productRent = db.Set<ProductRentEntity>().Find(productRentId);
        if (productRent == null)
        {
            throw new ValidationException($"指定的租赁目录#{productRentId}不存在!");
        }

        db.Remove<ProductRentEntity>(productRent);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"#{productRent.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductRentDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}