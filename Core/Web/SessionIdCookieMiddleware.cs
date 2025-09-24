using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.Web;

/// <summary>
/// 这个 middleware 的作用是保持 cookie 中一直有一个 session id，
/// 保持 WebSessionService的成员SessionId中的值 和 cookie 中的 session id 一致
/// </summary>
public class SessionIdCookieMiddleware
{
    private readonly RequestDelegate next;

    public SessionIdCookieMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context, [FromServices] WebSessionService webSessionService)
    {
        #region session id 处理

        String sessionId = webSessionService.GetSessionIdFromRequest(context.Request);

        if (webSessionService.IsValidSessionId(sessionId) == false)
        {
            sessionId = WebSessionService.NewSessionId();
        }

        webSessionService.SetSessionIdToResponse(context.Request, context.Response, sessionId);

        webSessionService.SessionId = sessionId;

        #endregion

        // 下面的逻辑中可以使用这个 WebSessionService.SessionId，所以应该 webSessionService.SessionId = sessionId; 之后调用

        await next.Invoke(context);
    }
}