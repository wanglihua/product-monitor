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
using Web.Models.Group;

namespace Web.Controllers;

[Auth(PassRes.SystemManage.Group)]
public class GroupController(
    Db db,
    UserLoginService userLoginService,
    ActionLogService actionLogService,
    UserAuthService userAuthService) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    [HttpPost]
    [Route("/api/group/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] GroupListParam param)
    {
        ValidationUtil.Validate<GroupListParamValidator, GroupListParam>(param);

        IQueryable<GroupEntity> query = db.Group.AsNoTracking();

        if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        {
            string nameQuery = param.NameQuery.Trim();
            query = query.Where(e => e.Name.Contains(nameQuery));
        }

        int groupCount = query.Count();

        // 排序字段
        Expression<Func<GroupEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (sortFieldParam == nameof(GroupListItem.CreateTime))
            {
                sortField = e => e.CreateTime;
            }

            if (sortFieldParam == nameof(GroupListItem.UpdateTime))
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

        var groupList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .Select(e => new {
                Id = e.Id,
                Name = e.Name,
                CreateTime = e.CreateTime,
                UpdateTime = e.UpdateTime,
            })
            .ToList();

        var groupItemList = groupList
            .Select(e => new GroupListItem {
                Id = e.Id,
                Name = e.Name,
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        long loginUserId = userLoginService.GetLoginUserId();
        bool isAdmin = userLoginService.IsAdmin(loginUserId);
        bool hasPassRes = userAuthService.UserHasPassRes(loginUserId, PassRes.SystemManage.PassRes, isAdmin);

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["groupList"] = groupItemList,
                ["groupCount"] = groupCount,
                ["hasPassRes"] = hasPassRes,
            },
        });
    }

    [HttpPost]
    [Route("/api/group/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamEmptyValidator, IdParam>(param);

        long groupId = param.Id;

        var model = new GroupDetailModel {
            Id = 0,
            Name = "",
        };

        if (groupId > 0)
        {
            var group = db.Group.Find(groupId);
            if (group is not null)
            {
                model = new GroupDetailModel {
                    Id = group.Id,
                    Name = group.Name,
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
    [Route("/api/group/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] GroupDetailModel model)
    {
        ValidationUtil.Validate<GroupDetailModelValidator, GroupDetailModel>(model);

        long groupId = model.Id;
        bool isCreate = groupId == 0;

        GroupEntity? group;
        if (isCreate == true)
        {
            group = new GroupEntity();
        }
        else
        {
            group = db.Group.Find(groupId);
            if (group is null)
            {
                throw new ValidationException($"指定的群组#{groupId}不存在!");
            }
        }

        group.Name = model.Name ?? "";

        if (isCreate == true)
        {
            db.Group.Add(group);
        }
        else
        {
            db.Group.Update(group);
        }

        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        if (isCreate == true)
        {
            string actionDesc = $"{group.Name}#{group.Id}";
            actionLogService.Add(loginUserId, ActionLogType.GroupCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"{group.Name}#{group.Id}";
            actionLogService.Add(loginUserId, ActionLogType.GroupUpdate, actionDesc);
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/group/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long groupId = param.Id;
        var group = db.Group.Find(groupId);
        if (group is null)
        {
            throw new ValidationException($"指定的群组#{groupId}不存在!");
        }

        var groupUserExist = db.GroupUser.AsNoTracking()
            .Where(e => e.GroupId == groupId)
            .OrderBy(e => e.GroupId)
            .Take(1)
            .SingleOrDefault();
        if (groupUserExist is not null)
        {
            throw new ValidationException("请先删除群组成员, 再删除群组!");
        }

        var groupPassResExist = db.GroupPassRes.AsNoTracking()
            .Where(e => e.GroupId == groupId)
            .OrderBy(e => e.GroupId)
            .Take(1)
            .SingleOrDefault();
        if (groupPassResExist is not null)
        {
            throw new ValidationException("请先清除群组授权, 再删除群组!");
        }

        db.Group.Remove(group);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"{group.Name}#{group.Id}";
        actionLogService.Add(loginUserId, ActionLogType.GroupDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}