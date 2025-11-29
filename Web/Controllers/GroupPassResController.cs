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
public class GroupPassResController(
    Db db,
    UserLoginService userLoginService,
    ActionLogService actionLogService) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    [HttpPost]
    [Route("/api/group-pass-res/group/list")]
    [Transactional(false)]
    public IActionResult GroupList([FromBody] PassResGroupListParam param)
    {
        ValidationUtil.Validate<PassResGroupListParamValidator, PassResGroupListParam>(param);

        long passResId = param.PassResId;
        var passRes = db.PassRes.Find(passResId);
        if (passRes is null)
        {
            throw new ValidationException($"指定的权限资源#{passResId}不存在!");
        }

        long passResCateId = passRes.CateId;
        var passResCate = db.PassResCate.Find(passResCateId);
        if (passResCate is null)
        {
            throw new ValidationException($"指定的权限资源类别#{passResCateId}不存在!");
        }

        IQueryable<GroupEntity> query = db.Group;

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

        var groupPassResList = db.GroupPassRes
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
                ["passResName"] = $"{passResCate.Name} {passRes.Name}",
            },
        });
    }

    private class GroupPassResJoinItem
    {
        public PassResCateEntity PassResCate { get; set; } = new();

        public PassResEntity PassRes { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/group-pass-res/pass/res/list")]
    [Transactional(false)]
    public IActionResult PassResList([FromBody] GroupPassResListParam param)
    {
        ValidationUtil.Validate<GroupPassResListParamValidator, GroupPassResListParam>(param);

        long groupId = param.GroupId;
        var group = db.Group.Find(groupId);
        if (group is null)
        {
            throw new ValidationException($"指定的群组#{groupId}不存在!");
        }

        IQueryable<GroupPassResJoinItem> query = db.Set<PassResEntity>()
            .Join(db.PassResCate,
                pr => pr.CateId,
                prc => prc.Id,
                (pr, prc) => new GroupPassResJoinItem {
                    PassRes = pr,
                    PassResCate = prc
                }
            );

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

        var passResList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var passResIdList = passResList.Select(e => e.PassRes.Id).ToList();
        var groupPassResList = db.GroupPassRes
            .Where(e => e.GroupId == groupId && passResIdList.Contains(e.PassResId))
            .ToList();

        var groupPassResItemList = new List<GroupPassResListItem>();
        foreach (var passRes in passResList)
        {
            long passResId = passRes.PassRes.Id;
            var groupPassRes = groupPassResList
                .Where(e => e.PassResId == passResId)
                .Take(1)
                .SingleOrDefault();

            groupPassResItemList.Add(new GroupPassResListItem {
                Id = passRes.PassRes.Id,
                PassResId = passRes.PassRes.Id,
                PassResCateName = passRes.PassResCate.Name,
                PassResName = passRes.PassRes.Name,
                Grant = groupPassRes != null,
                CreateTime = groupPassRes?.CreateTime.ToString("yyyy-MM-dd HH:mm") ?? "",
            });
        }

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

        var groupPassRes = db.GroupPassRes
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

        db.GroupPassRes.Add(groupPassRes);
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

        var groupPassRes = db.GroupPassRes
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