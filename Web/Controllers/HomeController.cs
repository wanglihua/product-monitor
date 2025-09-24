using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Web.Models;

namespace Web.Controllers;

public class HomeController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    public HomeController()
    {
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}