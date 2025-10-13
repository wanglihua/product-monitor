using Core.Auth;
using Core.Database;
using Domain;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Web.Controllers;

[Auth]
public class HomeController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;

    public HomeController(Db db)
    {
        this.db = db;
    }

    [HttpGet]
    [Route("/home/data")]
    [Transactional(false)]
    public IActionResult HomeData()
    {
        return Json(new Dictionary<string, object> {
            ["success"] = true,
            ["message"] = "success"
        });
    }
}