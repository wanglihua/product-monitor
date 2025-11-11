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
using Web.Models.GroupPassRes;

namespace Web.Controllers;

[Auth(PassRes.SystemManage.PassRes)]
public class GroupPassResController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly ActionLogService actionLogService;

    public GroupPassResController(
        Db db,
        UserLoginService userLoginService,
        ActionLogService actionLogService
    )
    {
        this.db = db;
        this.userLoginService = userLoginService;
        this.actionLogService = actionLogService;
    }

    private class PassResGroupListJoinItem
    {
        public GroupPassResEntity GroupPassRes { get; set; } = new();

        public GroupEntity Group { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/group-pass-res/group/list")]
    [Transactional(false)]
    public IActionResult GroupList([FromBody] PassResGroupListParam param)
    {
        ValidationUtil.Validate<PassResGroupListParamValidator, PassResGroupListParam>(param);

        long passResId = param.PassResId;
        var passRes = db.Set<PassResEntity>().Find(passResId);
        if (passRes is null)
        {
            throw new ValidationException($"指定的权限资源#{passResId}不存在!");
        }

        IQueryable<PassResGroupListJoinItem> query = db.Set<GroupPassResEntity>()
            .Join(db.Set<GroupEntity>(),
                gpr => gpr.GroupId,
                g => g.Id,
                (gpr, g) => new PassResGroupListJoinItem {
                    GroupPassRes = gpr,
                    Group = g,
                });

        query = query.Where(e => e.GroupPassRes.PassResId == passResId);

        if (string.IsNullOrWhiteSpace(param.GroupNameQuery) == false)
        {
            string groupNameQuery = param.GroupNameQuery.Trim();
            query = query.Where(e => e.Group.Name.Contains(groupNameQuery));
        }

        int passResGroupCount = query.Count();

        // 排序字段
        Expression<Func<PassResGroupListJoinItem, object>> sortField = e => e.GroupPassRes.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(GroupPassResListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.GroupPassRes.CreateTime;
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

        var passResGroupList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var passResGroupItemList = passResGroupList
            .Select(e => new PassResGroupListItem {
                Id = e.GroupPassRes.Id,
                GroupId = e.GroupPassRes.GroupId,
                GroupName = e.Group.Name,
                CreateTime = e.GroupPassRes.CreateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["passResGroupList"] = passResGroupItemList,
                ["passResGroupCount"] = passResGroupCount,
                ["passResName"] = passRes.Name,
            },
        });
    }

    private class GroupPassResJoinItem
    {
        public GroupPassResEntity GroupPassRes { get; set; } = new();

        public PassResEntity PassRes { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/group-pass-res/pass/res/list")]
    [Transactional(false)]
    public IActionResult PassResList([FromBody] GroupPassResListParam param)
    {
        ValidationUtil.Validate<GroupPassResListParamValidator, GroupPassResListParam>(param);

        long groupId = param.GroupId;
        var group = db.Set<GroupEntity>().Find(groupId);
        if (group is null)
        {
            throw new ValidationException($"指定的群组#{groupId}不存在!");
        }

        IQueryable<GroupPassResJoinItem> query = db.Set<GroupPassResEntity>()
            .Join(db.Set<PassResEntity>(),
                gpr => gpr.PassResId,
                pr => pr.Id,
                (gpr, pr) => new GroupPassResJoinItem {
                    GroupPassRes = gpr,
                    PassRes = pr,
                });

        query = query.Where(e => e.GroupPassRes.GroupId == groupId);

        if (string.IsNullOrWhiteSpace(param.PassResNameQuery) == false)
        {
            string passResNameQuery = param.PassResNameQuery.Trim();
            query = query.Where(e => e.PassRes.Name.Contains(passResNameQuery));
        }

        int groupPassResCount = query.Count();

        // 排序字段
        Expression<Func<GroupPassResJoinItem, object>> sortField = e => e.GroupPassRes.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(GroupPassResListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.GroupPassRes.CreateTime;
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

        var groupPassResList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var groupPassResItemList = groupPassResList
            .Select(e => new GroupPassResListItem {
                Id = e.GroupPassRes.Id,
                PassResId = e.GroupPassRes.PassResId,
                PassResName = e.PassRes.Name,
                CreateTime = e.GroupPassRes.CreateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["groupPassResList"] = groupPassResItemList,
                ["groupPassResCount"] = groupPassResCount,
                ["groupName"] = group.Name,
            },
        });
    }

    [HttpPost]
    [Route("/api/group-pass-res/grant")]
    [Transactional(true)]
    public IActionResult Grant([FromBody] GroupPassResGrantParam param)
    {
        ValidationUtil.Validate<GroupPassResGrantParamValidator, GroupPassResGrantParam>(param);

        long groupId = param.GroupId;
        long passResId = param.PassResId;

        var groupPassRes = db.Set<GroupPassResEntity>()
            .Where(e => e.GroupId == groupId && e.PassResId == passResId)
            .OrderBy(e => e.GroupId)
            .Take(1)
            .SingleOrDefault();

        if (groupPassRes is not null)
        {
            throw new ValidationException($"指定的群组授权已经存在! groupId:{groupId},passResId:{passResId}");
        }

        groupPassRes = new GroupPassResEntity {
            GroupId = groupId,
            PassResId = passResId,
        };

        groupPassRes.GroupId = param.GroupId;
        groupPassRes.PassResId = param.PassResId;

        db.Add<GroupPassResEntity>(groupPassRes);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"群组#{groupId},授权资源#{passResId}";
        actionLogService.Add(loginUserId, ActionLogType.GroupPassResGrant, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "授权成功！",
        });
    }

    [HttpPost]
    [Route("/api/group-pass-res/revoke")]
    [Transactional(true)]
    public IActionResult Revoke([FromBody] GroupPassResRevokeParam param)
    {
        ValidationUtil.Validate<GroupPassResRevokeParamValidator, GroupPassResRevokeParam>(param);

        long groupId = param.GroupId;
        long passResId = param.PassResId;

        var groupPassRes = db.Set<GroupPassResEntity>()
            .Where(e => e.GroupId == groupId && e.PassResId == passResId)
            .OrderBy(e => e.GroupId)
            .Take(1)
            .SingleOrDefault();

        if (groupPassRes is null)
        {
            throw new ValidationException($"指定的群组授权不存在! groupId:{groupId},passResId:{passResId}");
        }

        db.Remove<GroupPassResEntity>(groupPassRes);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"群组#{groupId},授权资源#{passResId}";
        actionLogService.Add(loginUserId, ActionLogType.GroupPassResRevoke, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "收权成功！",
        });
    }
}