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

    [HttpPost]
    [Route("/api/group-user/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] GroupUserListParam param)
    {
        IQueryable<GroupUserEntity> query = db.Set<GroupUserEntity>();

        long groupId = param.GroupId;
        query = query.Where(e => e.GroupId == groupId);

        int groupUserCount = query.Count();

        // 排序字段
        Expression<Func<GroupUserEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();

            if (sortFieldParam == nameof(GroupUserListItem.CreateTime).ToUpper())
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

        var groupUserList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var groupUserItemList = groupUserList
            .Select(e => new GroupUserListItem {
                Id = e.Id,
                GroupName = e.GroupId.ToString(),
                UserName = e.UserId.ToString(),
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["groupUserList"] = groupUserItemList,
                ["groupUserCount"] = groupUserCount,
            },
        });
    }

    [HttpPost]
    [Route("/api/group-user/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        long groupUserId = param.Id;

        var model = new GroupUserDetailModel {
            GroupId = 0,
            UserId = 0,
        };

        if (groupUserId > 0)
        {
            var groupUser = db.Set<GroupUserEntity>().Find(groupUserId);
            if (groupUser != null)
            {
                model = new GroupUserDetailModel {
                    GroupId = groupUser.GroupId,
                    UserId = groupUser.UserId,
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
        if (groupUserExist != null)
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
        if (groupUser == null)
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
            Message = "删除成功！",
        });
    }
}