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
using Web.Models.ProductEval;

namespace Web.Controllers;

[Auth(PassRes.ProjectManage.ProductEval)]
public class ProductEvalController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly ActionLogService actionLogService;

    public ProductEvalController(
        Db db,
        UserLoginService userLoginService,
        ActionLogService actionLogService
    )
    {
        this.db = db;
        this.userLoginService = userLoginService;
        this.actionLogService = actionLogService;
    }

    private class ProductEvalJoinItem
    {
        public ProductEvalEntity ProductEval { get; set; } = new();

        public CollegeEntity College { get; set; } = new();

        public ProductEntity Product { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/product-eval/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductEvalListParam param)
    {
        IQueryable<ProductEvalJoinItem> query = db.Set<ProductEvalEntity>()
            .Join(db.Set<CollegeEntity>(),
                ps => ps.CollegeId,
                c => c.Id,
                (ps, c) => new ProductEvalJoinItem {
                    ProductEval = ps,
                    College = c,
                })
            .Join(db.Set<ProductEntity>(),
                ps => ps.ProductEval.ProductId,
                p => p.Id,
                (ps, p) => new ProductEvalJoinItem {
                    ProductEval = ps.ProductEval,
                    College = ps.College,
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

        int productEvalCount = query.Count();

        // 排序字段
        Expression<Func<ProductEvalJoinItem, object>> sortField = e => e.ProductEval.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();

            if (sortFieldParam == nameof(ProductEvalListItem.CreateTime).ToUpper())
            {
                sortField = e => e.ProductEval.CreateTime;
            }

            if (sortFieldParam == nameof(ProductEvalListItem.UpdateTime).ToUpper())
            {
                sortField = e => e.ProductEval.UpdateTime;
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

        var productEvalList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var productEvalItemList = productEvalList
            .Select(e => new ProductEvalListItem {
                Id = e.ProductEval.Id,
                CollegeName = e.College.Name,
                ProductName = e.Product.Name,
                EvalTimes = e.ProductEval.EvalTimes,
                ServiceTimes = e.ProductEval.ServiceTimes,
                TotalDays = e.ProductEval.TotalDays,
                RemainDays = e.ProductEval.RemainDays,
                BeginDate = e.ProductEval.BeginDate.ToString("yyyy-MM-dd"),
                EndDate = e.ProductEval.EndDate.ToString("yyyy-MM-dd"),
                Description = e.ProductEval.Description,
                CreateTime = e.ProductEval.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.ProductEval.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["productEvalList"] = productEvalItemList,
                ["productEvalCount"] = productEvalCount,
            },
        });
    }

    [HttpPost]
    [Route("/api/product-eval/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        long productEvalId = param.Id;

        var model = new ProductEvalDetailModel {
            Id = 0,
            CollegeId = 0,
            ProductId = 0,
            EvalTimes = 0,
            ServiceTimes = 0,
            TotalDays = 0,
            RemainDays = 0,
            BeginDate = "",
            EndDate = "",
            Description = "",
        };

        if (productEvalId > 0)
        {
            var productEval = db.Set<ProductEvalEntity>().Find(productEvalId);
            if (productEval != null)
            {
                model = new ProductEvalDetailModel {
                    Id = productEval.Id,
                    CollegeId = productEval.CollegeId,
                    ProductId = productEval.ProductId,
                    EvalTimes = productEval.EvalTimes,
                    ServiceTimes = productEval.ServiceTimes,
                    TotalDays = productEval.TotalDays,
                    RemainDays = productEval.RemainDays,
                    BeginDate = productEval.BeginDate.ToString("yyyy-MM-dd"),
                    EndDate = productEval.EndDate.ToString("yyyy-MM-dd"),
                    Description = productEval.Description,
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
    [Route("/api/product-eval/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] ProductEvalDetailModel model)
    {
        ValidationUtil.Validate<ProductEvalDetailModelValidator, ProductEvalDetailModel>(model);

        long productEvalId = model.Id;
        bool isCreate = productEvalId == 0;

        ProductEvalEntity? productEval;
        if (isCreate == true)
        {
            productEval = new ProductEvalEntity();
        }
        else
        {
            productEval = db.Set<ProductEvalEntity>().Find(productEvalId);
            if (productEval == null)
            {
                throw new ValidationException($"指定的试用目录#{productEvalId}不存在!");
            }
        }

        productEval.CollegeId = model.CollegeId;
        productEval.ProductId = model.ProductId;
        productEval.EvalTimes = model.EvalTimes;
        productEval.ServiceTimes = model.ServiceTimes;
        productEval.TotalDays = model.TotalDays;
        productEval.RemainDays = model.RemainDays;
        productEval.BeginDate = DateTime.Parse(model.BeginDate).Date;
        productEval.EndDate = DateTime.Parse(model.EndDate).Date;
        productEval.Description = model.Description ?? "";

        if (isCreate == true)
        {
            db.Add<ProductEvalEntity>(productEval);
        }
        else
        {
            db.Update<ProductEvalEntity>(productEval);
        }

        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        if (isCreate == true)
        {
            string actionDesc = $"#{productEval.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductEvalCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"#{productEval.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductEvalUpdate, actionDesc);
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/product-eval/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long productEvalId = param.Id;
        var productEval = db.Set<ProductEvalEntity>().Find(productEvalId);
        if (productEval == null)
        {
            throw new ValidationException($"指定的试用目录#{productEvalId}不存在!");
        }

        db.Remove<ProductEvalEntity>(productEval);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"#{productEval.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductEvalDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}