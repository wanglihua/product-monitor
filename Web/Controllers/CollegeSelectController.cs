using Core.Auth;
using Core.Database;
using Domain;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Web.Models.CollegeSelect;

namespace Web.Controllers;

[Auth]
public class CollegeSelectController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;

    public CollegeSelectController(Db db)
    {
        this.db = db;
    }

    [HttpPost]
    [Route("/api/college/select/data")]
    [Transactional(false)]
    public IActionResult Data([FromBody] CollegeSelectDataQueryParam param)
    {
        IQueryable<CollegeEntity> query = db.Set<CollegeEntity>();

        if (string.IsNullOrWhiteSpace(param.Search) == false)
        {
            string search = param.Search;
            query = query.Where(e => e.Code.Contains(search) || e.Name.Contains(search));
        }

        var collegeList = query.OrderBy(e => e.Id).Take(10).ToList();

        var collegeItemList = collegeList
            .Select(e => new Dictionary<string, object> {
                ["value"] = e.Id,
                ["label"] = $"{e.Code} {e.Name}",
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