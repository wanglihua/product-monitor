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
using Web.Models.UserProvince;

namespace Web.Controllers;

[Auth(PassRes.BasicSetting.UserProvince)]
public class UserProvinceController(
    Db db,
    UserLoginService userLoginService,
    ActionLogService actionLogService) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    [HttpPost]
    [Route("/api/user-province/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] UserProvinceListParam param)
    {
        ValidationUtil.Validate<UserProvinceListParamValidator, UserProvinceListParam>(param);

        IQueryable<UserProvinceEntity> query = db.UserProvince;

        if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        {
            string nameQuery = param.NameQuery.Trim();
            query = query.Where(e => e.Name.Contains(nameQuery));
        }

        int userProvinceCount = query.Count();

        // 排序字段
        Expression<Func<UserProvinceEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(UserProvinceListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.CreateTime;
            }

            if (string.Compare(sortFieldParam, nameof(UserProvinceListItem.UpdateTime), StringComparison.OrdinalIgnoreCase) == 0)
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

        var userProvinceList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .Select(e => new {
                Id = e.Id,
                UserId = e.UserId,
                ProvinceType = e.ProvinceType,
                CreateTime = e.CreateTime,
                UpdateTime = e.UpdateTime,
            })
            .ToList();

        var userProvinceItemList = userProvinceList
            .Select(e => new UserProvinceListItem {
                Id = e.Id,
                UserId = e.UserId,
                ProvinceType = e.ProvinceType,
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
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
    public IActionResult Detail([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamEmptyValidator, IdParam>(param);

        long userProvinceId = param.Id;

        var model = new UserProvinceDetailModel {
            Id = 0,
            UserId = "",
            ProvinceType = "",
        };

        if (userProvinceId > 0)
        {
            var userProvince = db.UserProvince.Find(userProvinceId);
            if (userProvince is null)
            {
                throw new ValidationException("指定的#{userProvinceId}不存在!");
            }

            model = new UserProvinceDetailModel {
                Id = userProvince.Id,
                UserId = userProvince.UserId,
                ProvinceType = userProvince.ProvinceType,
            };
        }

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

        long userProvinceId = model.Id;
        bool isCreate = userProvinceId == 0;

        UserProvinceEntity? userProvince;
        if (isCreate == true)
        {
            userProvince = new UserProvinceEntity();
        }
        else
        {
            userProvince = db.UserProvince.Find(userProvinceId);
            if (userProvince is null)
            {
                throw new ValidationException($"指定的#{userProvinceId}不存在!");
            }
        }

        userProvince.UserId = model.UserId;
        userProvince.ProvinceType = model.ProvinceType;

        if (isCreate == true)
        {
            db.UserProvince.Add(userProvince);
        }

        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        if (isCreate == true)
        {
            string actionDesc = $"{userProvince.Name}#{userProvince.Id}";
            actionLogService.Add(loginUserId, ActionLogType.UserProvinceCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"{userProvince.Name}#{userProvince.Id}";
            actionLogService.Add(loginUserId, ActionLogType.UserProvinceUpdate, actionDesc);
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
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

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"{userProvince.Name}#{userProvince.Id}";
        actionLogService.Add(loginUserId, ActionLogType.UserProvinceDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}
