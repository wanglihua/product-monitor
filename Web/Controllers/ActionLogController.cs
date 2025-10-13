using Core.Auth;
using Core.Web;
using Domain;
using Domain.PassRes;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Web.Models.ActionLog;

namespace Web.Controllers;

[Auth(PassRes.SystemManage.ActionLog)]
public class ActionLogController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;

    public ActionLogController(Db db)
    {
        this.db = db;
    }

    private class ActionLogJoinItem
    {
        public ActionLogEntity ActionLog { get; set; } = new();

        public UserEntity User { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/action/log/list")]
    public IActionResult List([FromBody] ActionLogListParam param)
    {
        IQueryable<ActionLogJoinItem> query = db.Set<ActionLogEntity>()
            .Join(db.Set<UserEntity>(),
                actionLog => actionLog.UserId,
                user => user.Id,
                (actionLog, user) => new ActionLogJoinItem { ActionLog = actionLog, User = user }
            );

        if (string.IsNullOrWhiteSpace(param.LoginNameQuery) == false)
        {
            string loginNameQuery = param.LoginNameQuery.Trim();

            // 先过滤用户再过滤日志表, 日志数据量大的情况下会更快
            var userIdList = db.Set<UserEntity>()
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

        var actionLogList = query.OrderByDescending(e => e.ActionLog.Id)
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var actionLogItemList = actionLogList
            .Select(e => new ActionLogListItem {
                Id = e.ActionLog.Id,
                LoginName = e.User.LoginName,
                UserName = e.User.UserName,
                Type = e.ActionLog.Type.Name,
                ActionDesc = e.ActionLog.ActionDesc,
                CreateTime = e.ActionLog.CreateTime.ToString("yyyy-MM-dd HH:mm"),
            }).ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                { "actionLogList", actionLogItemList },
                { "actionLogCount", actionLogCount },
            }
        });
    }
}