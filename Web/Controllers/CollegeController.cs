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
using Web.Models.College;

namespace Web.Controllers;

[Auth(PassRes.BasicSetting.College)]
public class CollegeController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly ActionLogService actionLogService;

    public CollegeController(
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
    [Route("/api/college/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] CollegeListParam param)
    {
        ValidationUtil.Validate<CollegeListParamValidator, CollegeListParam>(param);

        IQueryable<CollegeEntity> query = db.Set<CollegeEntity>();

        if (string.IsNullOrWhiteSpace(param.CodeQuery) == false)
        {
            string codeQuery = param.CodeQuery.Trim();
            query = query.Where(p => p.Name.Contains(codeQuery));
        }

        if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        {
            string nameQuery = param.NameQuery.Trim();
            query = query.Where(p => p.Name.Contains(nameQuery));
        }

        int collegeCount = query.Count();

        // 排序字段
        Expression<Func<CollegeEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(CollegeListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.CreateTime;
            }

            if (string.Compare(sortFieldParam, nameof(CollegeListItem.UpdateTime), StringComparison.OrdinalIgnoreCase) == 0)
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

        var collegeList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var collegeItemList = collegeList
            .Select(e => new CollegeListItem {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                Province = e.Province.Name,
                Level = e.Level.Name,
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["collegeList"] = collegeItemList,
                ["collegeCount"] = collegeCount,
            },
        });
    }

    [HttpPost]
    [Route("/api/college/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamEmptyValidator, IdParam>(param);

        long collegeId = param.Id;

        var model = new CollegeDetailModel {
            Id = 0,
            Code = "",
            Name = "",
            Province = null,
            Level = null,
        };

        if (collegeId > 0)
        {
            var college = db.Set<CollegeEntity>().Find(collegeId);
            if (college != null)
            {
                model = new CollegeDetailModel {
                    Id = college.Id,
                    Code = college.Code,
                    Name = college.Name,
                    Province = college.Province.Id,
                    Level = college.Level.Id,
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
    [Route("/api/college/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] CollegeDetailModel model)
    {
        ValidationUtil.Validate<CollegeDetailModelValidator, CollegeDetailModel>(model);

        long collegeId = model.Id;
        bool isCreate = collegeId == 0;

        CollegeEntity? college;
        if (isCreate == true)
        {
            college = new CollegeEntity();
        }
        else
        {
            college = db.Set<CollegeEntity>().Find(collegeId);
            if (college == null)
            {
                throw new ValidationException($"指定的院校目录#{collegeId}不存在!");
            }
        }

        college.Code = model.Code ?? "";
        college.Name = model.Name ?? "";
        college.Province = ProvinceType.GetById(model.Province ?? 0);
        college.Level = CollegeLevelType.GetById(model.Level ?? 0);

        if (isCreate == true)
        {
            db.Add<CollegeEntity>(college);
        }
        else
        {
            db.Update<CollegeEntity>(college);
        }

        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        if (isCreate == true)
        {
            string actionDesc = $"{college.Name}#{college.Id}";
            actionLogService.Add(loginUserId, ActionLogType.CollegeCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"{college.Name}#{college.Id}";
            actionLogService.Add(loginUserId, ActionLogType.CollegeUpdate, actionDesc);
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/college/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long collegeId = param.Id;
        var college = db.Set<CollegeEntity>().Find(collegeId);
        if (college == null)
        {
            throw new ValidationException($"指定的院校目录#{collegeId}不存在!");
        }

        db.Remove<CollegeEntity>(college);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"{college.Name}#{college.Id}";
        actionLogService.Add(loginUserId, ActionLogType.CollegeDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}