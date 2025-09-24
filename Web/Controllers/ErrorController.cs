using Microsoft.AspNetCore.Mvc;
using NLog;
using Web.Models.Error;

namespace Web.Controllers;

public class ErrorController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
    
    [Route("/error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var model = new ErrorModel {
            
        };
        
        
        return View();
    }
}