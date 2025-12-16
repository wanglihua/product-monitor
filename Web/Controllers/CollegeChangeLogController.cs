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
using Web.Models.CollegeChangeLog;

namespace Web.Controllers;

[Auth(PassRes.BasicSetting.College)]
public class CollegeChangeLogController(
    Db db,
    UserLoginService userLoginService,
    ActionLogService actionLogService) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    [HttpPost]
    [Route("/api/college-change-log/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] CollegeChangeLogListParam param)
    {
        ValidationUtil.Validate<CollegeChangeLogListParamValidator, CollegeChangeLogListParam>(param);

        IQueryable<CollegeChangeLogEntity> query = db.CollegeChangeLog.AsNoTracking();

        int collegeChangeLogCount = query.Count();

        // 排序字段
        Expression<Func<CollegeChangeLogEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(CollegeChangeLogListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.CreateTime;
            }

            if (string.Compare(sortFieldParam, nameof(CollegeChangeLogListItem.UpdateTime), StringComparison.OrdinalIgnoreCase) == 0)
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

        var collegeChangeLogList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .Select(e => new {
                Id = e.Id,
                CollegeId = e.CollegeId,
                FormerName = e.FormerName,
                ChangeDesc = e.ChangeDesc,
                CreateTime = e.CreateTime,
                UpdateTime = e.UpdateTime,
            })
            .ToList();

        var collegeChangeLogItemList = collegeChangeLogList
            .Select(e => new CollegeChangeLogListItem {
                Id = e.Id,
                FormerName = e.FormerName,
                ChangeDesc = e.ChangeDesc,
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["collegeChangeLogList"] = collegeChangeLogItemList,
                ["collegeChangeLogCount"] = collegeChangeLogCount,
            },
        });
    }

    [HttpPost]
    [Route("/api/college-change-log/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamEmptyValidator, IdParam>(param);

        long collegeChangeLogId = param.Id;

        var model = new CollegeChangeLogDetailModel {
            Id = 0,
            CollegeId = 0,
            ChangeDesc = "",
            FormerName = "",
        };

        if (collegeChangeLogId > 0)
        {
            var collegeChangeLog = db.CollegeChangeLog.Find(collegeChangeLogId);
            if (collegeChangeLog is null)
            {
                throw new ValidationException("指定的院校更新记录#{collegeChangeLogId}不存在!");
            }

            model = new CollegeChangeLogDetailModel {
                Id = collegeChangeLog.Id,
                CollegeId = collegeChangeLog.CollegeId,
                FormerName = collegeChangeLog.FormerName,
                ChangeDesc = collegeChangeLog.ChangeDesc,
            };
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = model,
        });
    }

    [HttpPost]
    [Route("/api/college-change-log/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] CollegeChangeLogDetailModel model)
    {
        ValidationUtil.Validate<CollegeChangeLogDetailModelValidator, CollegeChangeLogDetailModel>(model);

        long collegeChangeLogId = model.Id;
        bool isCreate = collegeChangeLogId == 0;

        CollegeChangeLogEntity? collegeChangeLog;
        if (isCreate == true)
        {
            collegeChangeLog = new CollegeChangeLogEntity();
        }
        else
        {
            collegeChangeLog = db.CollegeChangeLog.Find(collegeChangeLogId);
            if (collegeChangeLog is null)
            {
                throw new ValidationException($"指定的院校更新记录#{collegeChangeLogId}不存在!");
            }
        }

        collegeChangeLog.CollegeId = model.CollegeId;
        collegeChangeLog.ChangeDesc = model.ChangeDesc ?? "";
        collegeChangeLog.FormerName = model.FormerName ?? "";

        if (isCreate == true)
        {
            db.CollegeChangeLog.Add(collegeChangeLog);
        }

        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        if (isCreate == true)
        {
            string actionDesc = $"{collegeChangeLog.FormerName}#{collegeChangeLog.Id}, #{collegeChangeLog.CollegeId}";
            actionLogService.Add(loginUserId, ActionLogType.CollegeChangeLogCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"{collegeChangeLog.FormerName}#{collegeChangeLog.Id}, #{collegeChangeLog.CollegeId}";
            actionLogService.Add(loginUserId, ActionLogType.CollegeChangeLogUpdate, actionDesc);
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/college-change-log/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long collegeChangeLogId = param.Id;
        var collegeChangeLog = db.CollegeChangeLog.Find(collegeChangeLogId);
        if (collegeChangeLog is null)
        {
            throw new ValidationException($"指定的院校更新记录#{collegeChangeLogId}不存在!");
        }

        db.CollegeChangeLog.Remove(collegeChangeLog);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"{collegeChangeLog.FormerName}#{collegeChangeLog.Id}, #{collegeChangeLog.CollegeId}";
        actionLogService.Add(loginUserId, ActionLogType.CollegeChangeLogDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}