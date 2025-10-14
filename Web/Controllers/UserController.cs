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
using Web.Models.User;

namespace Web.Controllers;

[Auth(PassRes.BasicSetting.User)]
public class UserController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly PasswordService passwordService;

    public UserController(Db db, PasswordService passwordService)
    {
        this.db = db;
        this.passwordService = passwordService;
    }

    [HttpPost]
    [Route("/api/user/list")]
    public IActionResult List([FromBody] UserListParam param)
    {
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

        int userCount = query.Count();

        // 排序字段
        Expression<Func<UserEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();
            if (sortFieldParam == nameof(UserListItem.LastLoginTime).ToUpper())
            {
                sortField = e => e.LastLoginTime!;
            }

            if (sortFieldParam == nameof(UserListItem.CreateTime).ToUpper())
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
            .ToList();

        var userItemList = userList
            .Select(e => new UserListItem {
                Id = e.Id,
                LoginName = e.LoginName,
                UserName = e.UserName,
                LastLoginTime = e.LastLoginTime?.ToString("yyyy-MM-dd HH:mm") ?? "",
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                { "userList", userItemList },
                { "userCount", userCount },
            }
        });
    }

    [HttpPost]
    [Route("/api/user/detail")]
    public IActionResult Detail([FromBody] IdParam param)
    {
        long userId = param.Id;

        var model = new UserDetailModel {
            Id = 0,
            LoginName = "",
            UserName = "",
            Password = "",
            PasswordAgain = "",
        };

        if (userId > 0)
        {
            var user = db.Set<UserEntity>().Find(userId);
            if (user != null)
            {
                model.Id = user.Id;
                model.LoginName = user.LoginName;
                model.UserName = user.UserName;
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
        }

        Transactional.Wrap(db, () => {
            UserEntity? user = null;
            string loginName = model.LoginName.Trim();
            if (isCreate)
            {
                user = new UserEntity();

                var otherUser = db.Set<UserEntity>()
                    .Where(e => e.LoginName == loginName)
                    .Take(1)
                    .FirstOrDefault();

                if (otherUser != null)
                {
                    throw new ValidationException($"登录名 {loginName} 已存在！");
                }
            }
            else
            {
                user = db.Set<UserEntity>().Find(userId);
                if (user == null)
                {
                    throw new ValidationException($"指定的用户#{userId}不存在！");
                }

                var otherUser = db.Set<UserEntity>()
                    .Where(e => e.LoginName == loginName && e.Id != userId)
                    .Take(1)
                    .FirstOrDefault();

                if (otherUser != null)
                {
                    throw new ValidationException($"登录名 {loginName} 已存在！");
                }
            }

            user.LoginName = loginName;
            user.UserName = model.UserName;
            if (string.IsNullOrWhiteSpace(model.Password) == false)
            {
                string salt = passwordService.GenerateSalt();

                user.Salt = salt;
                user.Password = passwordService.Encrypt(model.Password.Trim(), salt);
            }

            if (isCreate)
            {
                db.Add<UserEntity>(user);
            }
            else
            {
                db.Update<UserEntity>(user);
            }

            db.SaveChanges();
        });

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/user/delete")]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        Transactional.Wrap(db, () => {
            long userId = param.Id;
            var user = db.Set<UserEntity>().Find(userId);
            if (user == null)
            {
                throw new ValidationException($"指定的用户#{userId}不存在！");
            }

            var admin = db.Set<AdminUserEntity>()
                .Where(e => e.UserId == userId)
                .Take(1)
                .SingleOrDefault();
            if (admin != null)
            {
                throw new ValidationException("不能删除管理员用户！");
            }

            // db.Set<ActionLogEntity>()
            //     .Where(e => e.UserId == userId)
            //     .ExecuteDelete<ActionLogEntity>();

            // 删除用户
            db.Remove<UserEntity>(user);

            db.SaveChanges();
        });

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}