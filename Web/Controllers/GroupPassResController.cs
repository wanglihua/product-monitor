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

        IQueryable<GroupEntity> query = db.Set<GroupEntity>();

        if (string.IsNullOrWhiteSpace(param.GroupNameQuery) == false)
        {
            string groupNameQuery = param.GroupNameQuery.Trim();
            query = query.Where(e => e.Name.Contains(groupNameQuery));
        }

        int passResGroupCount = query.Count();

        // 排序字段
        Expression<Func<GroupEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(PassResGroupListItem.GroupName), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.Name;
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
            .ToList();

        var groupIdList = groupList.Select(e => e.Id).ToList();

        var groupPassResList = db.Set<GroupPassResEntity>()
            .Where(e => e.PassResId == passResId && groupIdList.Contains(e.GroupId))
            .ToList();


        var passResGroupItemList = new List<PassResGroupListItem>();
        foreach (var group in groupList)
        {
            long groupId = group.Id;
            var groupPassRes = groupPassResList
                .Where(e => e.GroupId == groupId)
                .Take(1)
                .SingleOrDefault();

            passResGroupItemList.Add(
                new PassResGroupListItem {
                    Id = group.Id,
                    GroupId = group.Id,
                    GroupName = group.Name,
                    Grant = groupPassRes != null,
                    CreateTime = groupPassRes?.CreateTime.ToString("yyyy-MM-dd HH:mm") ?? "",
                }
            );
        }

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
        public PassResCateEntity PassResCate { get; set; } = new();

        public PassResEntity PassRes { get; set; } = new();

        public GroupPassResEntity? GroupPassRes { get; set; }
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

        // 实际效果是左外联接, 也可以通过一个SQL视图来简化
        // 或用这里的方法做个通用方法简化, https://medium.com/@zabavnov/implementation-of-left-outer-join-for-entity-framework-b47469633e2f
        IQueryable<GroupPassResJoinItem> query = db.Set<PassResEntity>()
            .Join(db.Set<PassResCateEntity>(),
                pr => pr.CateId,
                prc => prc.Id,
                (pr, prc) => new { pr, prc }
            )
            .GroupJoin(db.Set<GroupPassResEntity>(),
                pr => pr.pr.Id,
                gpr => gpr.PassResId,
                (pr, gpr) => new { pr, gpr }
            )
            .SelectMany(
                g => g.gpr.DefaultIfEmpty(),
                (g, gpr) => new GroupPassResJoinItem {
                    PassResCate = g.pr.prc,
                    PassRes = g.pr.pr,
                    GroupPassRes = gpr,
                }
            );

        query = query.Where(e => e.GroupPassRes == null || e.GroupPassRes.GroupId == groupId);

        if (string.IsNullOrWhiteSpace(param.PassResNameQuery) == false)
        {
            string passResNameQuery = param.PassResNameQuery.Trim();
            query = query.Where(e => e.PassResCate.Name.Contains(passResNameQuery) || e.PassRes.Name.Contains(passResNameQuery));
        }

        int groupPassResCount = query.Count();

        // 排序字段
        Expression<Func<GroupPassResJoinItem, object>> sortField = e => e.PassRes.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(GroupPassResListItem.PassResName), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.PassRes.Name;
            }

            if (string.Compare(sortFieldParam, nameof(GroupPassResListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.GroupPassRes!.CreateTime;
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
                Id = e.PassRes.Id,
                PassResId = e.PassRes.Id,
                PassResCateName = e.PassResCate.Name,
                PassResName = e.PassRes.Name,
                Grant = e.GroupPassRes != null,
                CreateTime = e.GroupPassRes?.CreateTime.ToString("yyyy-MM-dd HH:mm") ?? "",
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