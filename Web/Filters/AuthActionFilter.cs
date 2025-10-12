using Microsoft.AspNetCore.Mvc.Filters;
using NLog;

namespace Web.Filters;

public class AuthActionFilter : IActionFilter
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    public void OnActionExecuting(ActionExecutingContext context)
    {
        log.Trace("AuthActionFilter.OnActionExecuting");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        log.Trace("AuthActionFilter.OnActionExecuted");
    }
}