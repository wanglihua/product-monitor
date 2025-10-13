using Microsoft.AspNetCore.Http;

namespace Core.Web;

/// <summary>
/// 自定义的 web session 实现
/// 
/// 保存数据到 "session" 中的时候，注意失效时间的 “滑动”,
/// 就是从 session 取数据的时候，可以再把这个数据重新往 session 里面放一下, 就实现了 “滑动”
/// 主要是 登录用户 id 的信息的滑动，别的 session 信息实际上都只是在 本地内存 中的缓存，缓存中没有了，就数据库中取一下。
///
/// 这个一定要设置成是 Scoped
/// </summary>
[Service(ServiceType.Scoped)]
public class WebSessionService
{
    public static readonly TimeSpan SessionTimeout = TimeSpan.FromMinutes(120);

    private static readonly string SessionIdCookieKey = "session_id";

    public string SessionId { get; set; } = string.Empty;

    public string GetSessionIdFromRequest(HttpRequest request)
    {
        string sessionId = request.Cookies[SessionIdCookieKey] ?? string.Empty;

        if (string.IsNullOrWhiteSpace(sessionId) == false && sessionId.Contains(","))
        {
            // 发现有一个key有两个值的情况

            sessionId = sessionId.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList()
                .OrderByDescending(e => e)
                .FirstOrDefault() ?? string.Empty;
        }

        return sessionId;
    }

    public string SetNewSessionIdToResponse(HttpRequest request, HttpResponse response)
    {
        string sessionId = NewSessionId();
        SessionId = sessionId;
        SetSessionIdToResponse(request, response, sessionId);

        return sessionId;
    }

    public static string NewSessionId()
    {
        string sessionId = DateTime.Now.ToString("yyMMddHHmmss") + Guid.NewGuid().ToString("N");
        return sessionId;
    }

    public static readonly int SessionIdLength = NewSessionId().Length;

    public bool IsValidSessionId(string sessionId)
    {
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            return false;
        }

        if (sessionId.Length < SessionIdLength)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 在cookie中，添加新的 session id，同时也清除和失效了原来的 session id
    /// </summary>
    public void SetSessionIdToResponse(HttpRequest request, HttpResponse response, string sessionId)
    {
        response.Cookies.Append(SessionIdCookieKey, sessionId, new CookieOptions() {
            HttpOnly = true,
            IsEssential = true,
            Path = "/",
        });
    }
}