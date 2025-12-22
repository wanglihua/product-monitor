using Core;
using Core.Auth;
using Core.Web;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class UserLoginService(
    Db db,
    IMemoryCache memoryCache,
    WebSessionService webSessionService,
    PasswordService passwordService,
    ActionLogService actionLogService,
    UserAuthService userAuthService)
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private static readonly TimeSpan LoginTimeout = TimeSpan.FromMinutes(60);
    private static readonly TimeSpan CacheTimeout = TimeSpan.FromSeconds(30); // 只是防止频繁调用而已，30秒钟够了

    public string GetUserLoginUrl()
    {
        return "/login";
    }

    public void ResetLoginTimeout(string sessionId)
    {
        DateTime now = DateTime.Now;
        db.WebSessionData
            .Where(e => e.SessionId == sessionId && e.Type == WebSessionDataType.LoginUserId)
            .ExecuteUpdate(e => e
                .SetProperty(x => x.UpdateTime, now)
            );
    }

    private string GetLoginUserIdCacheKey(string sessionId)
    {
        return $"login_user_id:{sessionId}";
    }

    public long GetLoginUserId()
    {
        string sessionId = webSessionService.SessionId;

        string cacheKey = GetLoginUserIdCacheKey(sessionId);
        if (memoryCache.TryGetValue(cacheKey, out long loginUserId))
        {
            return loginUserId;
        }

        var userLoginSessionData = db.WebSessionData.AsNoTracking()
            .Where(e => e.SessionId == sessionId && e.Type == WebSessionDataType.LoginUserId)
            .OrderBy(e => e.SessionId)
            .Take(1)
            .FirstOrDefault();
        if (userLoginSessionData is null)
        {
            return 0;
        }

        if (userLoginSessionData.UpdateTime < DateTime.Now - LoginTimeout)
        {
            return 0;
        }

        if (long.TryParse(userLoginSessionData.Data, out long userId) == false)
        {
            log.Error("web session data 中 login user id 数据格式不对!");

            return 0;
        }

        memoryCache.Set(cacheKey, userId, CacheTimeout);

        return userId;
    }

    private string GetIsAdminCacheKey(long userId)
    {
        return $"login_admin:{userId}";
    }

    public bool IsAdmin(long userId)
    {
        string cacheKey = GetIsAdminCacheKey(userId);

        if (memoryCache.TryGetValue(cacheKey, out bool isAdmin))
        {
            return isAdmin;
        }

        var admin = db.AdminUser.AsNoTracking()
            .Where(e => e.UserId == userId)
            .OrderBy(e => e.UserId)
            .Take(1)
            .SingleOrDefault();

        isAdmin = admin is not null;

        memoryCache.Set(cacheKey, isAdmin, CacheTimeout);

        return isAdmin;
    }

    /// <summary>
    /// 用户登录，并返回用户id
    /// </summary>
    /// <param name="loginName">登录名</param>
    /// <param name="password">密码</param>
    /// <param name="request"></param>
    /// <returns>登录成功的用户id，如果登录不成功就返回0</returns>
    public long Login(string loginName, string password, HttpRequest request)
    {
        var user = db.User.AsTracking() // 后面会更改用户最后登录时间等
            .Where(e => loginName == e.LoginName && e.Status != UserStatus.Deleted)
            .OrderBy(e => e.LoginName)
            .Take(1)
            .SingleOrDefault();

        if (user is null)
        {
            return 0;
        }

        if (user.Status != UserStatus.Enabled)
        {
            return 0;
        }

        if (user.Password != passwordService.Encrypt(password, user.Salt))
        {
            return 0;
        }

        return Login(user, request);
    }

    public long Login(UserEntity user, HttpRequest request)
    {
        if (user is null)
        {
            throw new ValidationException("登录用户不能为空！");
        }

        string sessionId = webSessionService.SessionId;
        var webSession = db.WebSession.AsNoTracking()
            .Where(e => e.SessionId == sessionId)
            .OrderBy(e => e.SessionId)
            .Take(1)
            .SingleOrDefault();
        if (webSession is null)
        {
            return 0;
        }

        long webSessionId = webSession.Id;
        var userLoginSessionData = db.WebSessionData.AsTracking()
            .Where(e => e.WebSessionId == webSessionId && e.Type == WebSessionDataType.LoginUserId)
            .OrderBy(e => e.WebSessionId)
            .Take(1)
            .FirstOrDefault();

        if (userLoginSessionData is null)
        {
            userLoginSessionData = new WebSessionDataEntity {
                WebSessionId = webSessionId,
                SessionId = sessionId,
                Type = WebSessionDataType.LoginUserId,
                Data = user.Id.ToString(),
            };

            db.WebSessionData.Add(userLoginSessionData);
        }
        else
        {
            userLoginSessionData.Data = user.Id.ToString();
        }

        // login log
        actionLogService.Add(user.Id, ActionLogType.UserLogin);

        // user 
        user.LastLoginTime = DateTime.Now;

        db.SaveChanges();

        // 登录时更新授权缓存
        userAuthService.BuildAuthCache();

        long userId = user.Id;

        var admin = db.AdminUser.AsNoTracking()
            .Where(e => e.UserId == userId)
            .OrderBy(e => e.UserId)
            .Take(1)
            .SingleOrDefault();

        bool isAdmin = admin is not null;

        // 设置登录 id 缓存 和 是否管理员缓存. 不是必须, 但双重保险
        memoryCache.Set(GetLoginUserIdCacheKey(sessionId), userId, CacheTimeout);
        memoryCache.Set(GetIsAdminCacheKey(userId), isAdmin, CacheTimeout);

        return userId;
    }

    public void Logout()
    {
        long loginUserId = GetLoginUserId();
        string isAdminCacheKey = GetIsAdminCacheKey(loginUserId);

        // 感觉这个比下面的 Remove() 更管用
        memoryCache.Set(isAdminCacheKey, false, TimeSpan.FromMilliseconds(1));
        memoryCache.Remove(isAdminCacheKey);

        string sessionId = webSessionService.SessionId;
        string loginUserIdCacheKey = GetLoginUserIdCacheKey(sessionId);

        // 感觉这个比下面的 Remove() 更管用
        memoryCache.Set(loginUserIdCacheKey, 0, TimeSpan.FromMilliseconds(1));
        memoryCache.Remove(loginUserIdCacheKey);

        // 会有级联删除关联 session data
        db.WebSession
            .Where(e => e.SessionId == sessionId)
            .ExecuteDelete();

        db.SaveChanges();
    }
}