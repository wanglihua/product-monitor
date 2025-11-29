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
using Web.Models.User;

namespace Web.Controllers;

[Auth(PassRes.SystemManage.User)]
public class UserController(
    Db db,
    UserLoginService userLoginService,
    PasswordService passwordService,
    AdminUserService adminUserService,
    ActionLogService actionLogService,
    UserAuthService userAuthService) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    [HttpPost]
    [Route("/api/user/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] UserListParam param)
    {
        ValidationUtil.Validate<UserListParamValidator, UserListParam>(param);

        IQueryable<UserEntity> query = db.User;

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

        int userCount = query.Count();

        // 排序字段
        Expression<Func<UserEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(UserListItem.LastLoginTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.LastLoginTime!;
            }

            if (string.Compare(sortFieldParam, nameof(UserListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.CreateTime;
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

        var userList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .Select(e => new {
                Id = e.Id,
                LoginName = e.LoginName,
                UserName = e.UserName,
                LastLoginTime = e.LastLoginTime,
                Status = e.Status,
                CreateTime = e.CreateTime,
            })
            .ToList();

        var userItemList = userList
            .Select(e => new UserListItem {
                Id = e.Id,
                LoginName = e.LoginName,
                UserName = e.UserName,
                LastLoginTime = e.LastLoginTime?.ToString("yyyy-MM-dd HH:mm") ?? "",
                Status = e.Status.Name,
                Enabled = e.Status == UserStatus.Enabled,
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        long loginUserId = userLoginService.GetLoginUserId();
        bool isAdmin = userLoginService.IsAdmin(loginUserId);
        bool hasPassRes = userAuthService.UserHasPassRes(loginUserId, PassRes.SystemManage.PassRes, isAdmin);

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["userList"] = userItemList,
                ["userCount"] = userCount,
                ["hasPassRes"] = hasPassRes,
            }
        });
    }

    [HttpPost]
    [Route("/api/user/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamEmptyValidator, IdParam>(param);

        long userId = param.Id;

        var model = new UserDetailModel {
            Id = 0,
            LoginName = "",
            UserName = "",
            Password = "",
            PasswordAgain = "",
            StatusId = null,
        };

        if (userId > 0)
        {
            var user = db.User.Find(userId);
            if (user is not null)
            {
                model.Id = user.Id;
                model.LoginName = user.LoginName;
                model.UserName = user.UserName;
                model.StatusId = user.Status.Id;
            }
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "success ",
            Data = model,
        });
    }

    [HttpPost]
    [Route("/api/user/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] UserDetailModel model)
    {
        long userId = model.Id;
        bool isCreate = userId == 0;

        if (isCreate)
        {
            ValidationUtil.Validate<UserDetailModelCreateValidator, UserDetailModel>(model);
        }
        else
        {
            ValidationUtil.Validate<UserDetailModelEditValidator, UserDetailModel>(model);
            //
            // string password = model.Password;
            // if (string.IsNullOrWhiteSpace(password) == false)
            // {
            //     password = password.Trim();
            //     if (password.Length < 6)
            //     {
            //         throw new ValidationException("密码名长度不能小于6！");
            //     }
            //
            //     if (password.Length > 20)
            //     {
            //         throw new ValidationException("密码名长度不能超过20！");
            //     }
            //
            //     string passwordAgain = model.Password;
            //     if (string.IsNullOrWhiteSpace(passwordAgain) == true)
            //     {
            //         throw new ValidationException("密码重输不能为空！");
            //     }
            //
            //     passwordAgain = passwordAgain.Trim();
            //
            //     if (passwordAgain.Length < 6)
            //     {
            //         throw new ValidationException("密码重输长度不能小于6！");
            //     }
            //
            //     if (passwordAgain.Length > 20)
            //     {
            //         throw new ValidationException("密码重输长度不能超过20！");
            //     }
            //
            //     if (passwordAgain != password)
            //     {
            //         throw new ValidationException("密码和密码重输不相等！");
            //     }
            // }
        }

        long loginUserId = userLoginService.GetLoginUserId();
        bool isAdminLogin = userLoginService.IsAdmin(loginUserId);

        UserEntity? user = null;
        string loginName = model.LoginName.Trim();
        if (isCreate == true)
        {
            user = new UserEntity();

            var otherUser = db.User
                .Where(e => e.LoginName == loginName)
                .OrderBy(e => e.LoginName)
                .Take(1)
                .FirstOrDefault();

            if (otherUser is not null)
            {
                throw new ValidationException($"登录名 {loginName} 已存在！");
            }
        }
        else
        {
            user = db.User.Find(userId);
            if (user is null)
            {
                throw new ValidationException($"指定的用户#{userId}不存在！");
            }

            var otherUser = db.User
                .Where(e => e.LoginName == loginName && e.Id != userId)
                .OrderBy(e => e.LoginName)
                .Take(1)
                .FirstOrDefault();

            if (otherUser is not null)
            {
                throw new ValidationException($"登录名 {loginName} 已存在！");
            }
        }

        user.LoginName = loginName;
        user.UserName = model.UserName;

        var userStatus = UserStatus.GetById(model.StatusId ?? 0);
        if (isCreate == false && userStatus == UserStatus.Disabled)
        {
            bool isAdminUser = adminUserService.IsAdminUser(user.Id);
            if (isAdminUser == true)
            {
                throw new ValidationException("管理员用户不能被禁用!");
            }
        }

        user.Status = userStatus;

        if (string.IsNullOrWhiteSpace(model.Password) == false)
        {
            // 判断是不是管理员, 管理员的密码, 只有管理员才能重置
            bool isAdminUser = adminUserService.IsAdminUser(user.Id); // 如果是新创建用户, 肯定会返回 false
            if (isAdminUser == true && isAdminLogin == false)
            {
                throw new ValidationException("只有管理员才能重新设置管理员密码!");
            }

            // 每次修改密码都重新生成 salt
            string salt = passwordService.GenerateSalt();

            user.Salt = salt;
            user.Password = passwordService.Encrypt(model.Password.Trim(), salt);
        }

        if (isCreate == true)
        {
            db.User.Add(user);
        }
        else
        {
            db.User.Update(user);
        }

        db.SaveChanges();

        if (isCreate == true)
        {
            string actionDesc = $"{user.LoginName}#{user.Id}";
            actionLogService.Add(loginUserId, ActionLogType.UserCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"{user.LoginName}#{user.Id}";
            actionLogService.Add(loginUserId, ActionLogType.UserUpdate, actionDesc);
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/user/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long userId = param.Id;
        var user = db.User.Find(userId);
        if (user is null)
        {
            throw new ValidationException($"指定的用户#{userId}不存在！");
        }

        var admin = db.AdminUser
            .Where(e => e.UserId == userId)
            .OrderBy(e => e.UserId)
            .Take(1)
            .SingleOrDefault();
        if (admin is not null)
        {
            throw new ValidationException("不能删除管理员用户！");
        }

        // todo 还有其他可能要删除的关联数据

        // 删除日志不是很好
        // db.ActionLog
        //     .Where(e => e.UserId == userId)
        //     .Log;

        // 删除用户
        db.Remove<UserEntity>(user);

        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"{user.LoginName}#{user.Id}";
        actionLogService.Add(loginUserId, ActionLogType.UserDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}