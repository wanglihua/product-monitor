using Core.Database;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Web.Controllers;

public class HomeController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    public HomeController()
    {
    }

    [HttpGet]
    [Route("/")]
    [Transactional(false)]
    public IActionResult Index()
    {
        return View();
    }
}