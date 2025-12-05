using Core.Auth;
using Core.Database;
using Core.Web;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[NoAuth]
public class ProductUpdateDataCollectController : Controller
{
    [HttpPost]
    [Route("/product/update/data/collect")]
    [Transactional(true)]
    public IActionResult Index()
    {
        return Json(new JsonResultData {
            Success = true,
            Message = "success",
        });
    }
}