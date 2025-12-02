using Core.Auth;
using Core.Database;
using Core.Util;
using Core.Web;
using Domain;
using Domain.PassRes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using Web.Models.ActionLog;

namespace Web.Controllers;

[Auth(PassRes.SystemManage.ActionLog)]
public class ActionLogController(Db db) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private class ActionLogJoinItem
    {
        public ActionLogEntity ActionLog { get; set; } = new();

        public UserEntity User { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/action/log/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ActionLogListParam param)
    {
        ValidationUtil.Validate<ActionLogListParamValidator, ActionLogListParam>(param);

        IQueryable<ActionLogJoinItem> query = db.ActionLog
            .Join(db.User,
                actionLog => actionLog.UserId,
                user => user.Id,
                (actionLog, user) => new ActionLogJoinItem {
                    ActionLog = actionLog,
                    User = user,
                })
            .AsNoTracking();

        if (param.ActionLogTypeQuery is not null && param.ActionLogTypeQuery != ActionLogType.None)
        {
            var actionLogTypeQuery = param.ActionLogTypeQuery;
            query = query.Where(e => e.ActionLog.Type == actionLogTypeQuery);
        }

        if (string.IsNullOrWhiteSpace(param.LoginNameQuery) == false)
        {
            string loginNameQuery = param.LoginNameQuery.Trim();

            // 先过滤用户再过滤日志表, 日志数据量大的情况下会更快
            var userIdList = db.User.AsNoTracking()
                .Where(e => e.LoginName.Contains(loginNameQuery) || e.UserName.Contains(loginNameQuery))
                .Select(e => e.Id)
                .ToList();

            if (userIdList.Count == 0)
            {
                userIdList = new List<long> { 0 }; // 放一个不存在的 user id 值
            }

            query = query.Where(e => userIdList.Contains(e.User.Id));
        }

        int actionLogCount = query.Count();

        // 只对id分页会很多, 数据量大的时候会很明显
        var actionLogIdList = query.OrderByDescending(e => e.ActionLog.Id)
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .Select(e => e.ActionLog.Id)
            .ToList();

        var actionLogList = query
            .Where(e => actionLogIdList.Contains(e.ActionLog.Id))
            .OrderByDescending(e => e.ActionLog.Id)
            .Select(e => new {
                Id = e.ActionLog.Id,
                LoginName = e.User.LoginName,
                UserName = e.User.UserName,
                Type = e.ActionLog.Type,
                ActionDesc = e.ActionLog.ActionDesc,
                CreateTime = e.ActionLog.CreateTime,
            })
            .ToList();

        var actionLogItemList = actionLogList
            .Select(e => new ActionLogListItem {
                Id = e.Id,
                LoginName = e.LoginName,
                UserName = e.UserName,
                Type = e.Type.Name,
                ActionDesc = e.ActionDesc,
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        var actionLogTypeList = new List<Dictionary<string, object>> {
            new Dictionary<string, object> {
                ["value"] = "",
                ["label"] = "类型查询",
            }
        };

        actionLogTypeList.AddRange(ActionLogType.GetList()
            .Select(e => new Dictionary<string, object> {
                ["value"] = e.Id,
                ["label"] = e.Name,
            })
        );

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                { "actionLogList", actionLogItemList },
                { "actionLogCount", actionLogCount },
                { "actionLogTypeList", actionLogTypeList },
            }
        });
    }
}