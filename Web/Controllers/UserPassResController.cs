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

    [HttpPost]
    [Route("/api/user-pass-res/user/list")]
    [Transactional(false)]
    public IActionResult UserList([FromBody] PassResUserListParam param)
    {
        ValidationUtil.Validate<PassResUserListParamValidator, PassResUserListParam>(param);

        long passResId = param.PassResId;

        var passRes = db.Set<PassResEntity>().Find(passResId);
        if (passRes is null)
        {
            throw new ValidationException($"指定的权限资源#{passResId}不存在!");
        }

        long passResCateId = passRes.CateId;
        var passResCate = db.Set<PassResCateEntity>().Find(passResCateId);
        if (passResCate is null)
        {
            throw new ValidationException($"指定的权限资源类别#{passResCateId}不存在!");
        }

        IQueryable<UserEntity> query = db.Set<UserEntity>();

        if (string.IsNullOrWhiteSpace(param.LoginNameQuery) == false)
        {
            string loginNameQuery = param.LoginNameQuery.Trim();
            query = query.Where(e => e.LoginName.Contains(loginNameQuery));
        }

        if (string.IsNullOrWhiteSpace(param.UserNameQuery) == false)
        {
            string userNameQuery = param.UserNameQuery.Trim();
            query = query.Where(e => e.UserName.Contains(userNameQuery));
        }

        int passResUserCount = query.Count();

        // 排序字段
        Expression<Func<UserEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(PassResUserListItem.LoginName), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.LoginName;
            }

            if (string.Compare(sortFieldParam, nameof(PassResUserListItem.UserName), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.UserName;
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

        var userList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var userIdList = userList.Select(e => e.Id).ToList();

        var userPassResList = db.Set<UserPassResEntity>()
            .Where(e => e.PassResId == passResId && userIdList.Contains(e.UserId))
            .ToList();

        var passResUserItemList = new List<PassResUserListItem>();
        foreach (var user in userList)
        {
            long userId = user.Id;
            var userPassRes = userPassResList
                .Where(e => e.UserId == userId)
                .Take(1)
                .SingleOrDefault();

            passResUserItemList.Add(new PassResUserListItem {
                Id = user.Id,
                UserId = user.Id,
                LoginName = user.LoginName,
                UserName = user.UserName,
                Grant = userPassRes != null,
                CreateTime = userPassRes?.CreateTime.ToString("yyyy-MM-dd HH:mm") ?? "",
            });
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["passResUserList"] = passResUserItemList,
                ["passResUserCount"] = passResUserCount,
                ["passResName"] = $"{passResCate.Name} {passRes.Name}",
            },
        });
    }

    private class UserPassResJoinItem
    {
        public PassResCateEntity PassResCate { get; set; } = new();

        public PassResEntity PassRes { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/user-pass-res/pass/res/list")]
    [Transactional(false)]
    public IActionResult PassResList([FromBody] UserPassResListParam param)
    {
        ValidationUtil.Validate<UserPassResListParamValidator, UserPassResListParam>(param);

        long userId = param.UserId;
        var user = db.Set<UserEntity>().Find(userId);
        if (user is null)
        {
            throw new ValidationException($"指定的用户#{userId}不存在!");
        }

        IQueryable<UserPassResJoinItem> query = db.Set<PassResEntity>()
            .Join(db.Set<PassResCateEntity>(),
                pr => pr.CateId,
                prc => prc.Id,
                (pr, prc) => new UserPassResJoinItem {
                    PassRes = pr,
                    PassResCate = prc
                }
            );

        if (string.IsNullOrWhiteSpace(param.PassResNameQuery) == false)
        {
            string passResNameQuery = param.PassResNameQuery.Trim();
            query = query.Where(e => e.PassResCate.Name.Contains(passResNameQuery) || e.PassRes.Name.Contains(passResNameQuery));
        }

        int userPassResCount = query.Count();

        // 排序字段
        Expression<Func<UserPassResJoinItem, object>> sortField = e => e.PassRes.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(UserPassResListItem.PassResName), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.PassRes.Name;
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

        var passResList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var passResIdList = passResList.Select(e => e.PassRes.Id).ToList();
        var userPassResList = db.Set<UserPassResEntity>()
            .Where(e => e.UserId == userId && passResIdList.Contains(e.PassResId))
            .ToList();

        var userPassResItemList = new List<UserPassResListItem>();
        foreach (var passRes in passResList)
        {
            long passResId = passRes.PassRes.Id;
            var userPassRes = userPassResList
                .Where(e => e.PassResId == passResId)
                .Take(1)
                .SingleOrDefault();

            userPassResItemList.Add(new UserPassResListItem {
                Id = passRes.PassRes.Id,
                PassResId = passRes.PassRes.Id,
                PassResCateName = passRes.PassResCate.Name,
                PassResName = passRes.PassRes.Name,
                Grant = userPassRes != null,
                CreateTime = userPassRes?.CreateTime.ToString("yyyy-MM-dd HH:mm") ?? "",
            });
        }

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