using Core.Auth;
using Core.Database;
using Core.Web;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[NoAuth]
public class ProductInstallDataCollectController : Controller
{
    [HttpPost]
    [Route("/product/install/data/collect")]
    [Transactional(true)]
    public IActionResult Index()
    {
        return Json(new JsonResultData {
            Success = true,
            Message = "success",
        });
    }
}