using System.Linq.Expressions;
using Core;
using Core.Auth;
using Core.Database;
using Core.Util;
using Core.Web;
using Domain;
using Domain.PassRes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
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

    private class PassResUserJoinItem
    {
        public UserEntity User { get; set; } = new();

        public UserPassResEntity? UserPassRes { get; set; }
    }

    [HttpPost]
    [Route("/api/user-pass-res/user/list")]
    [Transactional(false)]
    public IActionResult UserList([FromBody] PassResUserListParam param)
    {
        ValidationUtil.Validate<PassResUserListParamValidator, PassResUserListParam>(param);

        // 实际效果是左外联接, 也可以通过一个SQL视图来简化
        IQueryable<PassResUserJoinItem> query = db.Set<UserEntity>()
            .GroupJoin(db.Set<UserPassResEntity>(),
                u => u.Id,
                upr => upr.UserId,
                (u, upr) => new { u, upr })
            .SelectMany(
                g => g.upr.DefaultIfEmpty(),
                (g, upr) => new PassResUserJoinItem {
                    User = g.u,
                    UserPassRes = upr
                }
            );

        long passResId = param.PassResId;

        var passRes = db.Set<PassResEntity>().Find(passResId);
        if (passRes is null)
        {
            throw new ValidationException($"指定的权限资源#{passResId}不存在!");
        }

        query = query.Where(e => e.UserPassRes == null || e.UserPassRes.PassResId == passResId);

        if (string.IsNullOrWhiteSpace(param.LoginNameQuery) == false)
        {
            string loginNameQuery = param.LoginNameQuery.Trim();
            query = query.Where(e => e.User.LoginName.Contains(loginNameQuery));
        }

        if (string.IsNullOrWhiteSpace(param.LoginNameQuery) == false)
        {
            string loginNameQuery = param.LoginNameQuery.Trim();
            query = query.Where(e => e.User.LoginName.Contains(loginNameQuery));
        }

        int passResUserCount = query.Count();

        // 排序字段
        Expression<Func<PassResUserJoinItem, object>> sortField = e => e.User.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(PassResUserListItem.LoginName), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.User.LoginName;
            }

            if (string.Compare(sortFieldParam, nameof(PassResUserListItem.UserName), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.User.UserName;
            }
        }

        // 排序顺序
        string sortOrder = param.SortOrder;
        if (sortOrder == SortOrder.Descend) // 这里默认顺序排
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

        var passResUserItemList = userPassResList
            .Select(e => new PassResUserListItem {
                Id = e.User.Id,
                UserId = e.User.Id,
                LoginName = e.User.LoginName,
                UserName = e.User.UserName,
                CreateTime = e.UserPassRes?.CreateTime.ToString("yyyy-MM-dd HH:mm") ?? "",
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["passResUserList"] = passResUserItemList,
                ["passResUserCount"] = passResUserCount,
                ["passResName"] = passRes.Name,
            },
        });
    }

    private class UserPassResJoinItem
    {
        public UserPassResEntity UserPassRes { get; set; } = new();

        public PassResEntity PassRes { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/user-pass-res/pass/res/list")]
    [Transactional(false)]
    public IActionResult PassResList([FromBody] UserPassResListParam param)
    {
        ValidationUtil.Validate<UserPassResListParamValidator, UserPassResListParam>(param);

        IQueryable<UserPassResJoinItem> query = db.Set<UserPassResEntity>()
            .Join(db.Set<PassResEntity>(),
                upr => upr.UserId,
                pr => pr.Id,
                (upr, pr) => new UserPassResJoinItem {
                    UserPassRes = upr,
                    PassRes = pr,
                });

        long userId = param.UserId;
        var user = db.Set<UserEntity>().Find(userId);
        if (user is null)
        {
            throw new ValidationException($"指定的用户#{userId}不存在!");
        }

        query = query.Where(e => e.UserPassRes.UserId == userId);

        if (string.IsNullOrWhiteSpace(param.PassResNameQuery) == false)
        {
            string passResNameQuery = param.PassResNameQuery.Trim();
            query = query.Where(e => e.PassRes.Name.Contains(passResNameQuery));
        }

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
                PassResId = e.UserPassRes.PassResId,
                PassResName = e.PassRes.Name,
                CreateTime = e.UserPassRes.CreateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["userPassResList"] = userPassResItemList,
                ["userPassResCount"] = userPassResCount,
                ["loginName"] = user.LoginName,
                ["userName"] = user.UserName,
            },
        });
    }

    [HttpPost]
    [Route("/api/user-pass-res/grant")]
    [Transactional(true)]
    public IActionResult Grant([FromBody] UserPassResGrantParam param)
    {
        ValidationUtil.Validate<UserPassResGrantParamValidator, UserPassResGrantParam>(param);

        long userId = param.UserId;
        long passResId = param.PassResId;

        var userPassRes = db.Set<UserPassResEntity>()
            .Where(e => e.UserId == userId && e.PassResId == passResId)
            .OrderBy(e => e.UserId)
            .Take(1)
            .SingleOrDefault();

        if (userPassRes is not null)
        {
            throw new ValidationException($"指定的用户授权已经存在! userId:{userId},passResId:{passResId}");
        }

        userPassRes = new UserPassResEntity {
            UserId = userId,
            PassResId = passResId,
        };

        db.Add<UserPassResEntity>(userPassRes);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();

        string actionDesc = $"用户#{userId},授权资源#{passResId}";
        actionLogService.Add(loginUserId, ActionLogType.UserPassResGrant, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "授权成功！",
        });
    }

    [HttpPost]
    [Route("/api/user-pass-res/revoke")]
    [Transactional(true)]
    public IActionResult Revoke([FromBody] UserPassResRevokeParam param)
    {
        ValidationUtil.Validate<UserPassResRevokeParamValidator, UserPassResRevokeParam>(param);

        long userId = param.UserId;
        long passResId = param.PassResId;

        var userPassRes = db.Set<UserPassResEntity>()
            .Where(e => e.UserId == userId && e.PassResId == passResId)
            .OrderBy(e => e.UserId)
            .Take(1)
            .SingleOrDefault();

        if (userPassRes is null)
        {
            throw new ValidationException($"指定的用户授权不存在! userId:{userId},passResId:{passResId}");
        }

        db.Remove<UserPassResEntity>(userPassRes);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"用户#{userId},授权资源#{passResId}";
        actionLogService.Add(loginUserId, ActionLogType.UserPassResRevoke, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "收权成功！",
        });
    }
}