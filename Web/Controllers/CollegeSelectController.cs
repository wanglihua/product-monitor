using Core.Auth;
using Core.Config;
using Core.Database;
using Core.Util;
using Domain;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Web.Models.CollegeSelect;

namespace Web.Controllers;

[Auth]
public class CollegeSelectController(Db db) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    [HttpPost]
    [Route("/api/college/select/data")]
    [Transactional(false)]
    public IActionResult Data([FromBody] CollegeSelectDataQueryParam param)
    {
        ValidationUtil.Validate<CollegeSelectDataQueryParamValidator, CollegeSelectDataQueryParam>(param);

        IQueryable<CollegeEntity> query = db.College;

        if (string.IsNullOrWhiteSpace(param.Search) == false)
        {
            string search = param.Search.Trim();
            query = query.Where(e => e.Code.Contains(search) || e.Name.Contains(search));
        }

        var collegeList = query.OrderBy(e => e.Id).Take(SelectConfig.DropdownItemMaxCount).ToList();

        var collegeItemList = collegeList
            .Select(e => new Dictionary<string, object> {
                ["value"] = e.Id,
                ["label"] = e.Name,
                ["code"] = e.Code,
                ["name"] = e.Name,
            })
            .ToList();

        return Json(new Dictionary<string, object> {
            ["success"] = true,
            ["message"] = "success",
            ["collegeList"] = collegeItemList,
        });
    }
}