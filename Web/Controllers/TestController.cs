using Core.Auth;
using Core.Database;
using Core.Web;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Web.Controllers;

[NoAuth]
public class TestController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

#if DEBUG

    [HttpGet]
    [Route("/test")]
    [Transactional(true)]
    public IActionResult Index()
    {
        return Json(new JsonResultData {
            Success = true,
            Message = "success",
        });
    }

#endif
}