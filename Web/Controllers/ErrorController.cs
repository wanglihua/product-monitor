using Core;
using Core.Auth;
using Core.Database;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Web.Models.Error;

namespace Web.Controllers;

[NoAuth]
public class ErrorController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    [Route("/error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Transactional(false)]
    public IActionResult Error()
    {
        var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exp = feature?.Error;

        var model = new ErrorViewModel();
        if (exp != null)
        {
            if ((exp is ValidationException) == false)
            {
                log.Error(exp);
            }

            model.Message = exp.Message;
        }

        return View("Error", model);
    }
}