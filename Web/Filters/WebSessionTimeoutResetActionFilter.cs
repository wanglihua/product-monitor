using Core.Web;
using Microsoft.AspNetCore.Mvc.Filters;
using Service;

namespace Web.Filters;

public class WebSessionTimeoutResetActionFilter : IActionFilter
{
    private readonly WebSessionService webSessionService;
    private readonly WebSessionTimeoutResetService webSessionTimeoutResetService;
    private readonly UserLoginService userLoginService;

    public WebSessionTimeoutResetActionFilter(
        WebSessionService webSessionService,
        WebSessionTimeoutResetService webSessionTimeoutResetService,
        UserLoginService userLoginService
    )
    {
        this.webSessionService = webSessionService;
        this.webSessionTimeoutResetService = webSessionTimeoutResetService;
        this.userLoginService = userLoginService;
    }

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