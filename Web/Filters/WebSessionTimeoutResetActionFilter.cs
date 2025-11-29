using Core.Web;
using Microsoft.AspNetCore.Mvc.Filters;
using Service;

namespace Web.Filters;

public class WebSessionTimeoutResetActionFilter(
    WebSessionService webSessionService,
    WebSessionTimeoutResetService webSessionTimeoutResetService,
    UserLoginService userLoginService) : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        string sessionId = webSessionService.SessionId;

        if (string.IsNullOrEmpty(sessionId) == false)
        {
            webSessionTimeoutResetService.ResetTimeout(sessionId);
            userLoginService.ResetLoginTimeout(sessionId);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}