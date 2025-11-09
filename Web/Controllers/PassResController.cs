using System.Linq.Expressions;
using Core.Auth;
using Core.Database;
using Core.Web;
using Domain;
using Domain.PassRes;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Service;
using Web.Models.PassRes;

namespace Web.Controllers;

[Auth(PassRes.SystemManage.PassRes)]
public class PassResController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly PassResCateDataInitService passResCateDataInitService;
    private readonly PassResDataInitService passResDataInitService;
    private readonly ActionLogService actionLogService;

    public PassResController(
        Db db,
        UserLoginService userLoginService,
        PassResCateDataInitService passResCateDataInitService,
        PassResDataInitService passResDataInitService,
        ActionLogService actionLogService
    )
    {
        this.db = db;
        this.userLoginService = userLoginService;
        this.passResCateDataInitService = passResCateDataInitService;
        this.passResDataInitService = passResDataInitService;
        this.actionLogService = actionLogService;
    }

    [HttpPost]
    [Route("/api/pass-res/init/data")]
    [Transactional(true)]
    public IActionResult InitData()
    {
        long loginUserId = userLoginService.GetLoginUserId();

        passResCateDataInitService.InitData();
        passResDataInitService.InitData();

        actionLogService.Add(loginUserId, ActionLogType.PassResDataInit);

        return Json(new JsonResultData {
            Success = true,
            Message = "权限资源数据更新成功!",
        });
    }

    private class PassResJoinItem
    {
        public PassResEntity PassRes { get; set; } = new();

        public PassResCateEntity PassResCate { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/pass-res/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] PassResListParam param)
    {
        IQueryable<PassResJoinItem> query = db.Set<PassResEntity>()
            .Join(db.Set<PassResCateEntity>(),
                pr => pr.CateId,
                prc => prc.Id,
                (pr, prc) => new PassResJoinItem {
                    PassRes = pr,
                    PassResCate = prc,
                });

        if (param.CateIdQuery > 0)
        {
            var cateIdQuery = param.CateIdQuery;
            query = query.Where(e => e.PassRes.CateId == cateIdQuery);
        }

        if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        {
            string nameQuery = param.NameQuery.Trim();
            query = query.Where(e => e.PassRes.Name.Contains(nameQuery));
        }

        int passResCount = query.Count();

        /*
        // 排序字段
        Expression<Func<PassResJoinItem, object>> sortField = e => e.PassRes.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField.Trim().ToUpper();

            if (sortFieldParam == nameof(PassResListItem.CreateTime).ToUpper())
            {
                sortField = e => e.PassRes.CreateTime;
            }

            if (sortFieldParam == nameof(PassResListItem.UpdateTime).ToUpper())
            {
                sortField = e => e.PassRes.UpdateTime;
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
        */

        query = query.OrderBy(e => e.PassResCate.Id).ThenBy(e => e.PassRes.Id);

        var passResList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var passResItemList = passResList
            .Select(e => new PassResListItem {
                Id = e.PassRes.Id,
                CateName = e.PassResCate.Name,
                Name = e.PassRes.Name,
                CreateTime = e.PassRes.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.PassRes.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        var passResCateList = db.Set<PassResCateEntity>()
            .OrderBy(e => e.Id)
            .ToList();

        var passResCateListOptionList = new List<Dictionary<string, object>> {
            new Dictionary<string, object> {
                ["value"] = "",
                ["label"] = "类别查询",
            }
        };

        passResCateListOptionList.AddRange(passResCateList
            .Select(e => new Dictionary<string, object> {
                ["value"] = e.Id,
                ["label"] = e.Name,
            })
        );

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["passResList"] = passResItemList,
                ["passResCount"] = passResCount,
                ["passResCateList"] = passResCateListOptionList,
            },
        });
    }
}