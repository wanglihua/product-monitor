using Core.Auth;
using Core.Database;
using Core.Web;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[NoAuth]
public class ProductUrlDataCollectController : Controller
{
    [HttpPost]
    [Route("/product/url/data/collect")]
    [Transactional(true)]
    public IActionResult Index()
    {
        return Json(new JsonResultData {
            Success = true,
            Message = "success",
        });
    }
} 