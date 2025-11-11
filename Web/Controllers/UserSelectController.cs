using Core.Auth;
using Core.Config;
using Core.Database;
using Core.Util;
using Domain;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Web.Models.UserSelect;

namespace Web.Controllers;

[Auth]
public class UserSelectController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;

    public UserSelectController(Db db)
    {
        this.db = db;
    }

    [HttpPost]
    [Route("/api/user/select/data")]
    [Transactional(false)]
    public IActionResult Data([FromBody] UserSelectDataQueryParam param)
    {
        ValidationUtil.Validate<UserSelectDataQueryParamValidator, UserSelectDataQueryParam>(param);

        IQueryable<UserEntity> query = db.Set<UserEntity>();

        if (string.IsNullOrWhiteSpace(param.Search) == false)
        {
            string search = param.Search.Trim();
            query = query.Where(e => e.LoginName.Contains(search) || e.UserName.Contains(search));
        }

        var userList = query.OrderBy(e => e.Id).Take(SelectConfig.DropdownItemMaxCount).ToList();

        var userItemList = userList
            .Select(e => new Dictionary<string, object> {
                ["value"] = e.Id,
                ["label"] = $"{e.LoginName} {e.UserName}",
            })
            .ToList();

        return Json(new Dictionary<string, object> {
            ["success"] = true,
            ["message"] = "success",
            ["userList"] = userItemList,
        });
    }
}