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
    
    private class ProductRentJoinItem
    {
        public ProductRentEntity ProductRent { get; set; } = new();

        public CollegeEntity College { get; set; } = new();

        public ProductEntity Product { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/product-rent/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductRentListParam param)
    {
        IQueryable<ProductRentJoinItem> query = db.Set<ProductRentEntity>()
            .Join(db.Set<CollegeEntity>(),
                pr => pr.CollegeId,
                c => c.Id,
                (pr, c) => new ProductRentJoinItem {
                    ProductRent = pr,
                    College = c,
                })
            .Join(db.Set<ProductEntity>(),
                pr => pr.ProductRent.ProductId,
                p => p.Id,
                (pr, p) => new ProductRentJoinItem {
                    ProductRent = pr.ProductRent,
                    College = pr.College,
                    Product = p,
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

        int productRentCount = query.Count();

        // 排序字段
        Expression<Func<ProductRentJoinItem, object>> sortField = e => e.ProductRent.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();

            if (sortFieldParam == nameof(ProductRentListItem.CreateTime).ToUpper())
            {
                sortField = e => e.ProductRent.CreateTime;
            }

            if (sortFieldParam == nameof(ProductRentListItem.UpdateTime).ToUpper())
            {
                sortField = e => e.ProductRent.UpdateTime;
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
                Id = e.ProductRent.Id,
                CollegeId = e.ProductRent.CollegeId,
                ProductId = e.ProductRent.ProductId,
                RentTimes = e.ProductRent.RentTimes,
                ServiceTimes = e.ProductRent.ServiceTimes,
                TotalDays = e.ProductRent.TotalDays,
                RemainDays = e.ProductRent.RemainDays,
                BeginDate = e.ProductRent.BeginDate.ToString("yyyy-MM-dd"),
                EndDate = e.ProductRent.EndDate.ToString("yyyy-MM-dd"),
                Description = e.ProductRent.Description,
                CreateTime = e.ProductRent.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.ProductRent.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
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