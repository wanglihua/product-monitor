using Core.Auth;
using Core.Config;
using Core.Database;
using Domain;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Web.Models.ProductSelect;

namespace Web.Controllers;

[Auth]
public class ProductSelectController(Db db) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    [HttpPost("/api/product/select/data")]
    [Transactional(false)]
    public IActionResult Data([FromBody] ProductSelectDataQueryParam param)
    {
        IQueryable<ProductEntity> query = db.Product;

        if (string.IsNullOrWhiteSpace(param.Search) == false)
        {
            string search = param.Search.Trim();
            query = query.Where(e => e.Name.Contains(search));
        }

        var productList = query
            .OrderBy(e => e.Id)
            .Take(SelectConfig.DropdownItemMaxCount)
            .Select(e => new {
                Id = e.Id,
                Name = e.Name,
            })
            .ToList();

        var productItemList = productList
            .Select(e => new Dictionary<string, object>() {
                ["value"] = e.Id,
                ["label"] = e.Name,
            })
            .ToList();

        return Json(new Dictionary<string, object> {
            ["success"] = true,
            ["message"] = "success",
            ["productList"] = productItemList,
        });
    }
}