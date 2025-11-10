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
using Web.Models.UserPassRes;

namespace Web.Controllers;

[Auth(PassRes.SystemManage.PassRes)]
public class UserPassResController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly ActionLogService actionLogService;

    public UserPassResController(
        Db db,
        UserLoginService userLoginService,
        ActionLogService actionLogService
    )
    {
        this.db = db;
        this.userLoginService = userLoginService;
        this.actionLogService = actionLogService;
    }

    private class UserPassResJoinItem
    {
        public UserPassResEntity UserPassRes { get; set; } = new();

        public UserEntity User { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/user-pass-res/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] UserPassResListParam param)
    {
        ValidationUtil.Validate<UserPassResListParamValidator, UserPassResListParam>(param);

        IQueryable<UserPassResJoinItem> query = db.Set<UserPassResEntity>()
            .Join(db.Set<UserEntity>(),
                upr => upr.UserId,
                u => u.Id,
                (upr, u) => new UserPassResJoinItem {
                    UserPassRes = upr,
                    User = u,
                });

        long passResId = param.PassResId;
        query = query.Where(e => e.UserPassRes.PassResId == passResId);


        // if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        // {
        //     string nameQuery = param.NameQuery.Trim();
        //     query = query.Where(e => e.Name.Contains(nameQuery));
        // }

        int userPassResCount = query.Count();

        // 排序字段
        Expression<Func<UserPassResJoinItem, object>> sortField = e => e.UserPassRes.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(UserPassResListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.UserPassRes.CreateTime;
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

        var userPassResList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var userPassResItemList = userPassResList
            .Select(e => new UserPassResListItem {
                Id = e.UserPassRes.Id,
                UserId = e.UserPassRes.UserId,
                PassResId = e.UserPassRes.PassResId,
                CreateTime = e.UserPassRes.CreateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["userPassResList"] = userPassResItemList,
                ["userPassResCount"] = userPassResCount,
            },
        });
    }

    [HttpPost]
    [Route("/api/user-pass-res/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamEmptyValidator, IdParam>(param);

        long userPassResId = param.Id;

        var model = new UserPassResDetailModel {
            Id = 0,
            UserId = 0,
            PassResId = 0,
        };

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = model,
        });
    }

    [HttpPost]
    [Route("/api/user-pass-res/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] UserPassResDetailModel model)
    {
        ValidationUtil.Validate<UserPassResDetailModelValidator, UserPassResDetailModel>(model);

        long userPassResId = model.Id;
        bool isCreate = userPassResId == 0;

        UserPassResEntity? userPassRes;
        if (isCreate == true)
        {
            userPassRes = new UserPassResEntity();
        }
        else
        {
            userPassRes = db.Set<UserPassResEntity>().Find(userPassResId);
            if (userPassRes == null)
            {
                throw new ValidationException($"指定的用户授权#{userPassResId}不存在!");
            }
        }

        userPassRes.UserId = model.UserId;
        userPassRes.PassResId = model.PassResId;

        if (isCreate == true)
        {
            db.Add<UserPassResEntity>(userPassRes);
        }
        else
        {
            db.Update<UserPassResEntity>(userPassRes);
        }

        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();

        string actionDesc = $"#{userPassRes.Id}";
        actionLogService.Add(loginUserId, ActionLogType.UserPassResGrant, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/user-pass-res/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long userPassResId = param.Id;
        var userPassRes = db.Set<UserPassResEntity>().Find(userPassResId);
        if (userPassRes == null)
        {
            throw new ValidationException($"指定的用户授权#{userPassResId}不存在!");
        }

        db.Remove<UserPassResEntity>(userPassRes);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"#{userPassRes.Id}";
        actionLogService.Add(loginUserId, ActionLogType.UserPassResRevoke, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}