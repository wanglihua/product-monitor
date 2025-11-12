using System.Linq.Expressions;
using Core.Auth;
using Core.Database;
using Core.Util;
using Core.Web;
using Domain;
using Domain.PassRes;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Service;
using Web.Models.AuthResult;

namespace Web.Controllers;

[Auth(PassRes.SystemManage.PassRes)]
public class AuthResultController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserAuthService userAuthService;

    public AuthResultController(
        Db db,
        UserAuthService userAuthService
    )
    {
        this.db = db;
        this.userAuthService = userAuthService;
    }

    [HttpPost]
    [Route("/api/auth-result/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] AuthResultListParam param)
    {
        ValidationUtil.Validate<AuthResultListParamValidator, AuthResultListParam>(param);

        IQueryable<AuthResultEntity> query = db.Set<AuthResultEntity>();

        if (string.IsNullOrWhiteSpace(param.LoginNameQuery) == false)
        {
            string loginNameQuery = param.LoginNameQuery.Trim();
            query = query.Where(e => e.LoginName.Contains(loginNameQuery));
        }

        if (string.IsNullOrWhiteSpace(param.UserNameQuery) == false)
        {
            string userNameQuery = param.UserNameQuery.Trim();
            query = query.Where(e => e.UserName.Contains(userNameQuery));
        }

        if (string.IsNullOrWhiteSpace(param.PassResCateNameQuery) == false)
        {
            string passResCateNameQuery = param.PassResCateNameQuery.Trim();
            query = query.Where(e => e.PassResCateName.Contains(passResCateNameQuery));
        }

        if (string.IsNullOrWhiteSpace(param.PassResNameQuery) == false)
        {
            string passResNameQuery = param.PassResNameQuery.Trim();
            query = query.Where(e => e.PassResName.Contains(passResNameQuery));
        }

        if (string.IsNullOrWhiteSpace(param.GroupNameQuery) == false)
        {
            string groupNameQuery = param.GroupNameQuery.Trim();
            query = query.Where(e => e.GroupName.Contains(groupNameQuery));
        }

        int authResultCount = query.Count();

        // 排序字段
        Expression<Func<AuthResultEntity, object>> sortField = e => e.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(AuthResultListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
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

        var authResultList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .ToList();

        var authResultItemList = authResultList
            .Select(e => new AuthResultListItem {
                Id = e.Id,
                PassResCateName = e.PassResCateName,
                PassResName = e.PassResName,
                LoginName = e.LoginName,
                UserName = e.UserName,
                GroupName = e.GroupName,
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["authResultList"] = authResultItemList,
                ["authResultCount"] = authResultCount,
            },
        });
    }

    [HttpPost]
    [Route("/api/auth-result/refresh/auth/cache")]
    [Transactional(false)]
    public IActionResult RefreshAuthCache()
    {
        userAuthService.BuildAuthCache();

        return Json(new JsonResultData {
            Success = true,
            Message = "更新授权缓存成功!",
        });
    }
}