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

    [HttpPost]
    [Route("/api/product-eval/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductEvalListParam param)
    {
        IQueryable<ProductEvalEntity> query = db.Set<ProductEvalEntity>();

        // if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        // {
        //     string nameQuery = param.NameQuery.Trim();
        //     query = query.Where(p => p.Name.Contains(nameQuery));
        // }

        int productEvalCount = query.Count();

        // 排序字段
        Expression<Func<ProductEvalEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();

            if (sortFieldParam == nameof(ProductEvalListItem.CreateTime).ToUpper())
            {
                sortField = e => e.CreateTime;
            }

            if (sortFieldParam == nameof(ProductEvalListItem.UpdateTime).ToUpper())
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

        var productEvalList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var productEvalItemList = productEvalList
            .Select(e => new ProductEvalListItem {
                Id = e.Id,
                CollegeId = e.CollegeId,
                ProductId = e.ProductId,
                EvalTimes = e.EvalTimes,
                SaleTimes = e.SaleTimes,
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
            SaleTimes = 0,
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
                    SaleTimes = productEval.SaleTimes,
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
        productEval.SaleTimes = model.SaleTimes;
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