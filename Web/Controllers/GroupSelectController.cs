using Core.Auth;
using Core.Config;
using Core.Database;
using Core.Util;
using Domain;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Web.Models.GroupSelect;

namespace Web.Controllers;

[Auth]
public class GroupSelectController(Db db) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    [HttpPost]
    [Route("/api/group/select/data")]
    [Transactional(false)]
    public IActionResult Data([FromBody] GroupSelectDataQueryParam param)
    {
        ValidationUtil.Validate<GroupSelectDataQueryParamValidator, GroupSelectDataQueryParam>(param);

        IQueryable<GroupEntity> query = db.Group;

        if (string.IsNullOrWhiteSpace(param.Search) == false)
        {
            string search = param.Search.Trim();
            query = query.Where(e => e.Name.Contains(search));
        }

        var groupList = query.OrderBy(e => e.Id).Take(SelectConfig.DropdownItemMaxCount).ToList();

        var groupItemList = groupList
            .Select(e => new Dictionary<string, object> {
                ["value"] = e.Id,
                ["label"] = e.Name,
            })
            .ToList();

        return Json(new Dictionary<string, object> {
            ["success"] = true,
            ["message"] = "success",
            ["groupList"] = groupItemList,
        });
    }
}