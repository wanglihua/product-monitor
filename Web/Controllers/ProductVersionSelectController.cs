using Core.Auth;
using Core.Database;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using Web.Models.ProductVersionSelect;

namespace Web.Controllers;

[Auth]
public class ProductVersionSelectController(Db db) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    [HttpPost]
    [Route("/api/product/version/select/data")]
    [Transactional(false)]
    public IActionResult Data([FromBody] ProductVersionSelectDataQueryParam param)
    {
        long productId = param.ProductId;
        var query = db.ProductVersion.AsNoTracking()
            .Where(e => e.ProductId == productId);

        var productVersionList = query
            .OrderBy(e => e.Version)
            .ToList();

        var productVersionItemList = productVersionList
            .Select(e => new Dictionary<string, object> {
                ["value"] = e.Version,
                ["label"] = e.Version,
            })
            .ToList();

        return Json(new Dictionary<string, object> {
            ["success"] = true,
            ["message"] = "success",
            ["productVersionList"] = productVersionItemList,
        });
    }
}