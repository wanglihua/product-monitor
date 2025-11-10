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
    [Route("/api/group-pass-res/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] GroupPassResListParam param)
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
    [Route("/api/group-pass-res/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamEmptyValidator, IdParam>(param);

        long groupPassResId = param.Id;

        var model = new GroupPassResDetailModel {
            Id = 0,
            GroupId = 0,
            PassResId = 0,
        };

        if (groupPassResId > 0)
        {
            var groupPassRes = db.Set<GroupPassResEntity>().Find(groupPassResId);
            if (groupPassRes != null)
            {
                model = new GroupPassResDetailModel {
                    Id = groupPassRes.Id,
                    GroupId = groupPassRes.GroupId,
                    PassResId = groupPassRes.PassResId,
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
    [Route("/api/group-pass-res/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] GroupPassResDetailModel model)
    {
        ValidationUtil.Validate<GroupPassResDetailModelValidator, GroupPassResDetailModel>(model);

        long groupPassResId = model.Id;
        bool isCreate = groupPassResId == 0;

        GroupPassResEntity? groupPassRes;
        if (isCreate == true)
        {
            groupPassRes = new GroupPassResEntity();
        }
        else
        {
            groupPassRes = db.Set<GroupPassResEntity>().Find(groupPassResId);
            if (groupPassRes == null)
            {
                throw new ValidationException($"指定的群组授权#{groupPassResId}不存在!");
            }
        }

        groupPassRes.GroupId = model.GroupId;
        groupPassRes.PassResId = model.PassResId;

        if (isCreate == true)
        {
            db.Add<GroupPassResEntity>(groupPassRes);
        }
        else
        {
            db.Update<GroupPassResEntity>(groupPassRes);
        }

        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"#{groupPassRes.Id}";
        actionLogService.Add(loginUserId, ActionLogType.GroupPassResGrant, actionDesc);


        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/group-pass-res/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long groupPassResId = param.Id;
        var groupPassRes = db.Set<GroupPassResEntity>().Find(groupPassResId);
        if (groupPassRes == null)
        {
            throw new ValidationException($"指定的群组授权#{groupPassResId}不存在!");
        }

        db.Remove<GroupPassResEntity>(groupPassRes);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"#{groupPassRes.Id}";
        actionLogService.Add(loginUserId, ActionLogType.GroupPassResRevoke, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}