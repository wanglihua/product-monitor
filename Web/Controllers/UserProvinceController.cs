using System.Linq.Expressions;
using Core;
using Core.Auth;
using Core.Database;
using Core.Util;
using Core.Web;
using Domain;
using Domain.PassRes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using Service;
using Web.Models.UserProvince;

namespace Web.Controllers;

[Auth(PassRes.SystemManage.UserProvince)]
public class UserProvinceController(
    Db db,
    UserLoginService userLoginService,
    ActionLogService actionLogService) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private class UserProvinceJoinItem
    {
        public UserProvinceEntity UserProvince { get; set; } = new();

        public UserEntity User { get; set; } = new();
    }


    [HttpPost]
    [Route("/api/user-province/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] UserProvinceListParam param)
    {
        ValidationUtil.Validate<UserProvinceListParamValidator, UserProvinceListParam>(param);

        IQueryable<UserProvinceJoinItem> query = db.UserProvince
            .Join(db.User,
                up => up.UserId,
                u => u.Id,
                (up, u) => new UserProvinceJoinItem {
                    UserProvince = up,
                    User = u,
                }
            )
            .AsNoTracking();

        if (string.IsNullOrWhiteSpace(param.UserNameQuery) == false)
        {
            string nameQuery = param.UserNameQuery.Trim();
            query = query.Where(e => e.User.LoginName.Contains(nameQuery) || e.User.UserName.Contains(nameQuery));
        }

        var provinceTypeQuery = ProvinceType.GetById(param.ProvinceType);
        if (provinceTypeQuery != ProvinceType.None)
        {
            query = query.Where(e => e.UserProvince.ProvinceType == provinceTypeQuery);
        }

        int userProvinceCount = query.Count();

        // 排序字段
        Expression<Func<UserProvinceJoinItem, object>> sortField = e => e.UserProvince.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(UserProvinceListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.UserProvince.CreateTime;
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

        var userProvinceList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .Select(e => new {
                Id = e.UserProvince.Id,
                e.User.LoginName,
                e.User.UserName,
                e.UserProvince.ProvinceType,
                e.UserProvince.CreateTime,
            })
            .ToList();

        var userProvinceItemList = userProvinceList
            .Select(e => new UserProvinceListItem {
                Id = e.Id,
                LoginName = e.LoginName,
                UserName = e.UserName,
                ProvinceType = e.ProvinceType.Name,
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["userProvinceList"] = userProvinceItemList,
                ["userProvinceCount"] = userProvinceCount,
            },
        });
    }

    [HttpPost]
    [Route("/api/user-province/detail")]
    [Transactional(false)]
    public IActionResult Detail()
    {
        var model = new UserProvinceDetailModel {
            UserId = 0,
            ProvinceType = ProvinceType.None.Id,
        };

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = model,
        });
    }

    [HttpPost]
    [Route("/api/user-province/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] UserProvinceDetailModel model)
    {
        ValidationUtil.Validate<UserProvinceDetailModelValidator, UserProvinceDetailModel>(model);

        long userId = model.UserId;
        var user = db.User.Find(userId);
        if (user is null)
        {
            throw new ValidationException($"指定的用户#{userId}不存在！");
        }

        var provinceType = ProvinceType.GetById(model.ProvinceType);

        var userProvince = new UserProvinceEntity {
            UserId = userId,
            ProvinceType = provinceType,
        };

        db.UserProvince.Add(userProvince);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"{user.LoginName}#{provinceType.Name}";
        actionLogService.Add(loginUserId, ActionLogType.UserProvinceCreate, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "添加成功！",
        });
    }

    [HttpPost]
    [Route("/api/user-province/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long userProvinceId = param.Id;
        var userProvince = db.UserProvince.Find(userProvinceId);
        if (userProvince is null)
        {
            throw new ValidationException($"指定的#{userProvinceId}不存在!");
        }

        db.UserProvince.Remove(userProvince);
        db.SaveChanges();

        long userId = userProvince.UserId;
        var user = db.User.Find(userProvince.UserId);
        if (user is null)
        {
            throw new ValidationException($"指定的用户#{userId}不存在！");
        }

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"{user.LoginName}#{userProvince.ProvinceType.Name}";
        actionLogService.Add(loginUserId, ActionLogType.UserProvinceDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "移除成功！",
        });
    }
}