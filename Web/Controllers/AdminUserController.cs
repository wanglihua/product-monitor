using System.Linq.Expressions;
using Core;
using Core.Auth;
using Core.Database;
using Core.Util;
using Core.Web;
using Domain;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Service;
using Web.Models.AdminUser;

namespace Web.Controllers;

[Auth(AdminOnly = true)]
public class AdminUserController(
    Db db,
    UserLoginService userLoginService,
    ActionLogService actionLogService) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private class AdminUserJoinItem
    {
        public AdminUserEntity Admin { get; set; } = new();

        public UserEntity User { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/admin-user/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] AdminUserListParam param)
    {
        ValidationUtil.Validate<AdminUserListParamValidator, AdminUserListParam>(param);

        IQueryable<AdminUserJoinItem> query = db.AdminUser
            .Join(db.User,
                admin => admin.UserId,
                user => user.Id,
                (admin, user) => new AdminUserJoinItem { Admin = admin, User = user }
            );

        if (string.IsNullOrWhiteSpace(param.LoginNameQuery) == false)
        {
            string loginNameQuery = param.LoginNameQuery.Trim();
            query = query.Where(e => e.User.LoginName.Contains(loginNameQuery));
        }

        if (string.IsNullOrWhiteSpace(param.UserNameQuery) == false)
        {
            string userNameQuery = param.UserNameQuery.Trim();
            query = query.Where(e => e.User.UserName.Contains(userNameQuery));
        }

        int adminCount = query.Count();

        // 排序字段
        Expression<Func<AdminUserJoinItem, object>> sortField = e => e.Admin.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;
            if (string.Compare(sortFieldParam, nameof(AdminUserListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.Admin.CreateTime;
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

        var adminList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .Select(e => new {
                Id = e.Admin.Id,
                LoginName = e.User.LoginName,
                UserName = e.User.UserName,
                CreateTime = e.Admin.CreateTime,
            })
            .ToList();

        var adminItemList = adminList
            .Select(e => new AdminUserListItem {
                Id = e.Id,
                LoginName = e.LoginName,
                UserName = e.UserName,
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                { "adminList", adminItemList },
                { "adminCount", adminCount },
            }
        });
    }

    [HttpPost]
    [Route("/api/admin-user/add/save")]
    [Transactional(true)]
    public IActionResult AddSave([FromBody] AdminUserAddSaveParam param)
    {
        ValidationUtil.Validate<AdminUserAddSaveParamValidator, AdminUserAddSaveParam>(param);

        long loginUserId = userLoginService.GetLoginUserId();

        string loginName = param.LoginName.Trim();
        var user = db.User
            .Where(e => e.LoginName == loginName)
            .OrderBy(e => e.LoginName)
            .Take(1)
            .SingleOrDefault();
        if (user is null)
        {
            throw new ValidationException($"指定的登录名 {loginName} 不存在！");
        }

        long userId = user.Id;
        var admin = db.AdminUser
            .Where(e => e.UserId == userId)
            .OrderBy(e => e.UserId)
            .Take(1)
            .SingleOrDefault();
        if (admin is not null)
        {
            throw new ValidationException($"指定的登录名 {loginName} 已经是管理员！");
        }

        admin = new AdminUserEntity {
            UserId = userId,
        };

        db.AdminUser.Add(admin);
        db.SaveChanges();

        string actionDesc = $"{user.LoginName}#{user.Id}";
        actionLogService.Add(loginUserId, ActionLogType.AdminUserAdd, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "添加成功！",
        });
    }

    [HttpPost]
    [Route("/api/admin-user/remove")]
    [Transactional(true)]
    public IActionResult Remove([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long loginUserId = userLoginService.GetLoginUserId();

        long adminId = param.Id;

        var admin = db.AdminUser.Find(adminId);
        if (admin is null)
        {
            throw new ValidationException($"指定的管理员#{adminId}不存在！");
        }

        long userId = admin.UserId;
        var user = db.User.Find(userId);
        if (user is null)
        {
            throw new ValidationException($"指定的用户#{userId}不存在!");
        }

        var adminOther = db.AdminUser
            .Where(e => e.Id != adminId)
            .OrderBy(e => e.Id)
            .Take(1)
            .SingleOrDefault();
        if (adminOther is null)
        {
            throw new ValidationException("必需至少保留一位管理员！");
        }

        db.AdminUser.Remove(admin);
        db.SaveChanges();

        string actionDesc = $"{user.LoginName}#{user.Id}";
        actionLogService.Add(loginUserId, ActionLogType.AdminUserRemove, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "移除成功！",
        });
    }
}