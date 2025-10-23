using Core.Auth;
using Core.Database;
using Core.Web;
using Domain;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Web.Models.ProductSelect;

namespace Web.Controllers;

[Auth]
public class ProductSelectController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;

    public ProductSelectController(Db db)
    {
        this.db = db;
    }

    [HttpPost("/api/product/select/data")]
    [Transactional]
    public IActionResult Data([FromBody] ProductSelectDataQueryParam param)
    {
        IQueryable<ProductEntity> query = db.Set<ProductEntity>();

        if (string.IsNullOrWhiteSpace(param.NameQuery) == false)
        {
            string nameQuery = param.NameQuery.Trim();
            query = query.Where(e => e.Name.Contains(nameQuery));
        }

        var productList = query.OrderBy(e => e.Id).Take(10).ToList();

        var productOptionList = productList
            .Select(e => new SelectOption {
                Value = e.Id,
                Label = e.Name,
            })
            .ToList();

        return Json(new Dictionary<string, object> {
            ["success"] = true,
            ["message"] = "success",
            ["productList"] = productOptionList,
        });
    }
}