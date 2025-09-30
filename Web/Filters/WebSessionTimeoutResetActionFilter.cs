using Core.Web;
using Microsoft.AspNetCore.Mvc.Filters;
using Service;

namespace Web.Filters;

public class WebSessionTimeoutResetActionFilter : IActionFilter
{
    private readonly WebSessionService webSessionService;
    private readonly WebSessionTimeoutResetService webSessionTimeoutResetService;

    public WebSessionTimeoutResetActionFilter(
        WebSessionService webSessionService,
        WebSessionTimeoutResetService webSessionTimeoutResetService
    )
    {
        this.webSessionService = webSessionService;
        this.webSessionTimeoutResetService = webSessionTimeoutResetService;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        string webSessionId = webSessionService.SessionId;

        if (string.IsNullOrEmpty(webSessionId) == false)
        {
            webSessionTimeoutResetService.ResetTimeout(webSessionId);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}