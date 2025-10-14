using Core.Auth;
using Core.Database;
using Core.Web;
using Domain.PassRes;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Web.Controllers;

[Auth(PassRes.BasicSetting.PassRes)]
public class PassResController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    [HttpPost]
    [Route("/api/pass/res/init/data")]
    [Transactional(true)]
    public IActionResult InitData()
    {
        return Json(new JsonResultData {
        });
    }

    [HttpPost]
    [Route("/api/pass/res/list")]
    [Transactional(false)]
    public IActionResult List()
    {
        return Json(new JsonResultData {
        });
    }
}