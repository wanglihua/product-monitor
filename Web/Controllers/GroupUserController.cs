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
using Web.Models.GroupUser;

namespace Web.Controllers;

[Auth(PassRes.SystemManage.Group)]
public class GroupUserController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly ActionLogService actionLogService;

    public GroupUserController(
        Db db,
        UserLoginService userLoginService,
        ActionLogService actionLogService
    )
    {
        this.db = db;
        this.userLoginService = userLoginService;
        this.actionLogService = actionLogService;
    }

    private class GroupJointItem
    {
        public GroupUserEntity GroupUser { get; set; } = new();

        public UserEntity User { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/group-user/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] GroupUserListParam param)
    {
        ValidationUtil.Validate<GroupUserListParamValidator, GroupUserListParam>(param);

        long groupId = param.GroupId;

        var group = db.Set<GroupEntity>().Find(groupId);
        if (group is null)
        {
            throw new ValidationException($"指定的群组#{groupId}不存在!");
        }

        IQueryable<GroupJointItem> query = db.Set<GroupUserEntity>()
            .Join(db.Set<UserEntity>(),
                gu => gu.UserId,
                u => u.Id,
                (gu, u) => new GroupJointItem {
                    GroupUser = gu,
                    User = u
                }
            );

        query = query.Where(e => e.GroupUser.GroupId == groupId);

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

        int groupUserCount = query.Count();

        // 排序字段
        Expression<Func<GroupJointItem, object>> sortField = e => e.GroupUser.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(GroupUserListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.GroupUser.CreateTime;
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

        var groupUserList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .Select(e => new {
                Id = e.GroupUser.Id,
                LoginName = e.User.LoginName,
                UserName = e.User.UserName,
                CreateTime = e.GroupUser.CreateTime,
            })
            .ToList();

        var groupUserItemList = groupUserList
            .Select(e => new GroupUserListItem {
                Id = e.Id,
                LoginName = e.LoginName,
                UserName = e.UserName,
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        string groupName = group.Name;

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["groupUserList"] = groupUserItemList,
                ["groupUserCount"] = groupUserCount,
                ["groupName"] = groupName,
            },
        });
    }

    [HttpPost]
    [Route("/api/group-user/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] GroupUserDetailParam param)
    {
        ValidationUtil.Validate<GroupUserDetailParamValidator, GroupUserDetailParam>(param);

        long groupId = param.GroupId;

        var model = new GroupUserDetailModel {
            GroupId = groupId,
            UserId = 0,
        };

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = model,
        });
    }

    [HttpPost]
    [Route("/api/group-user/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] GroupUserDetailModel model)
    {
        ValidationUtil.Validate<GroupUserDetailModelValidator, GroupUserDetailModel>(model);

        long groupId = model.GroupId;
        long userId = model.UserId;

        var groupUserExist = db.Set<GroupUserEntity>()
            .Where(e => e.GroupId == groupId && e.UserId == userId)
            .OrderBy(e => e.GroupId)
            .Take(1)
            .SingleOrDefault();
        if (groupUserExist is not null)
        {
            throw new ValidationException("指定的用户已经在群组中, 不能重复添加!");
        }

        var groupUser = new GroupUserEntity {
            GroupId = groupId,
            UserId = userId
        };

        db.Add<GroupUserEntity>(groupUser);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"group:#{groupUser.GroupId}, user:#{groupUser.UserId}";
        actionLogService.Add(loginUserId, ActionLogType.GroupUserAdd, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/group-user/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long groupUserId = param.Id;
        var groupUser = db.Set<GroupUserEntity>().Find(groupUserId);
        if (groupUser is null)
        {
            throw new ValidationException($"指定的群组成员#{groupUserId}不存在!");
        }

        db.Remove<GroupUserEntity>(groupUser);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"group:#{groupUser.GroupId}, user:#{groupUser.UserId}";
        actionLogService.Add(loginUserId, ActionLogType.GroupUserRemove, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "移除成功！",
        });
    }

    [HttpPost]
    [Route("/api/group-user/empty")]
    [Transactional(true)]
    public IActionResult Empty([FromBody] GroupUserEmptyParam param)
    {
        ValidationUtil.Validate<GroupUserEmptyParamValidator, GroupUserEmptyParam>(param);

        long groupId = param.GroupId;
        var group = db.Set<GroupEntity>().Find(groupId);
        if (group is null)
        {
            throw new ValidationException($"指定的群组#{groupId}不存在!");
        }

        db.Set<GroupUserEntity>()
            .Where(e => e.GroupId == groupId)
            .ExecuteDelete();

        return Json(new JsonResultData {
            Success = true,
            Message = "清空成功！",
        });
    }
}