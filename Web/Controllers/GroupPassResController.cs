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
    public IActionResult GroupList([FromBody] GroupPassResListParam param)
    {
        ValidationUtil.Validate<GroupPassResListParamValidator, GroupPassResListParam>(param);

        IQueryable<GroupPassResEntity> query = db.Set<GroupPassResEntity>();

        // if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        // {
        //     string nameQuery = param.NameQuery.Trim();
        //     query = query.Where(e => e.Name.Contains(nameQuery));
        // }

        int groupPassResCount = query.Count();

        // 排序字段
        Expression<Func<GroupPassResEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(GroupPassResListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
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

        var groupPassResList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var groupPassResItemList = groupPassResList
            .Select(e => new GroupPassResListItem {
                Id = e.Id,
                GroupId = e.GroupId,
                PassResId = e.PassResId,
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["groupPassResList"] = groupPassResItemList,
                ["groupPassResCount"] = groupPassResCount,
            },
        });
    }

    [HttpPost]
    [Route("/api/group-pass-res/pass/res/list")]
    [Transactional(false)]
    public IActionResult PassResList([FromBody] GroupPassResListParam param)
    {
        ValidationUtil.Validate<GroupPassResListParamValidator, GroupPassResListParam>(param);

        IQueryable<GroupPassResEntity> query = db.Set<GroupPassResEntity>();

        // if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        // {
        //     string nameQuery = param.NameQuery.Trim();
        //     query = query.Where(e => e.Name.Contains(nameQuery));
        // }

        int groupPassResCount = query.Count();

        // 排序字段
        Expression<Func<GroupPassResEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(GroupPassResListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
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

        var groupPassResList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var groupPassResItemList = groupPassResList
            .Select(e => new GroupPassResListItem {
                Id = e.Id,
                GroupId = e.GroupId,
                PassResId = e.PassResId,
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["groupPassResList"] = groupPassResItemList,
                ["groupPassResCount"] = groupPassResCount,
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

        if (groupPassRes != null)
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

        if (groupPassRes == null)
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