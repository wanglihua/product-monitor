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
public class UserLoginService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private static readonly TimeSpan LoginTimeout = TimeSpan.FromMinutes(60);

    private readonly Db db;
    private readonly IMemoryCache memoryCache;
    private readonly WebSessionService webSessionService;
    private readonly PasswordService passwordService;
    private readonly ActionLogService actionLogService;

    public UserLoginService(
        Db db,
        IMemoryCache memoryCache,
        WebSessionService webSessionService,
        PasswordService passwordService,
        ActionLogService actionLogService
    )
    {
        this.db = db;
        this.memoryCache = memoryCache;
        this.webSessionService = webSessionService;
        this.passwordService = passwordService;
        this.actionLogService = actionLogService;
    }

    public string GetUserLoginUrl()
    {
        return "/login";
    }

    public void ResetLoginTimeout(string sessionId)
    {
        DateTime now = DateTime.Now;
        db.Set<WebSessionDataEntity>()
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

        var userLoginSessionData = db.Set<WebSessionDataEntity>()
            .Where(e => e.SessionId == sessionId && e.Type == WebSessionDataType.LoginUserId)
            .OrderBy(e => e.SessionId)
            .Take(1)
            .FirstOrDefault();
        if (userLoginSessionData == null)
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

        memoryCache.Set(cacheKey, userId, TimeSpan.FromSeconds(20)); // 只是防止频繁调用而已，20秒钟够了

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

        var admin = db.Set<AdminUserEntity>()
            .Where(e => e.UserId == userId)
            .OrderBy(e => e.UserId)
            .Take(1)
            .SingleOrDefault();

        isAdmin = admin != null;

        memoryCache.Set(cacheKey, isAdmin, TimeSpan.FromSeconds(20)); // 只是防止频繁调用而已，20秒钟够了

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
        var user = db.Set<UserEntity>()
            .Where(e => loginName == e.LoginName)
            .OrderBy(e => e.LoginName)
            .Take(1)
            .SingleOrDefault();

        if (user == null)
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
        if (user == null)
        {
            throw new ValidationException("登录用户不能为空！");
        }

        string sessionId = webSessionService.SessionId;
        var webSession = db.Set<WebSessionEntity>()
            .Where(e => e.SessionId == sessionId)
            .OrderBy(e => e.SessionId)
            .Take(1)
            .SingleOrDefault();
        if (webSession == null)
        {
            return 0;
        }

        long webSessionId = webSession.Id;
        var userLoginSessionData = db.Set<WebSessionDataEntity>()
            .Where(e => e.WebSessionId == webSessionId && e.Type == WebSessionDataType.LoginUserId)
            .OrderBy(e => e.WebSessionId)
            .Take(1)
            .FirstOrDefault();

        if (userLoginSessionData == null)
        {
            userLoginSessionData = new WebSessionDataEntity {
                WebSessionId = webSessionId,
                SessionId = sessionId,
                Type = WebSessionDataType.LoginUserId,
                Data = user.Id.ToString(),
            };

            db.Add<WebSessionDataEntity>(userLoginSessionData);
        }
        else
        {
            userLoginSessionData.Data = user.Id.ToString();
            db.Update<WebSessionDataEntity>(userLoginSessionData);
        }

        // login log
        actionLogService.Add(user.Id, ActionLogType.UserLogin);

        // user 
        user.LastLoginTime = DateTime.Now;
        db.Update<UserEntity>(user);

        db.SaveChanges();

        return user.Id;
    }

    public void Logout()
    {
        string sessionId = webSessionService.SessionId;
        string loginUserIdCacheKey = GetLoginUserIdCacheKey(sessionId);
        memoryCache.Remove(loginUserIdCacheKey);

        long loginUserId = GetLoginUserId();
        string isAdminCacheKey = GetIsAdminCacheKey(loginUserId);
        memoryCache.Remove(isAdminCacheKey);

        // 会有级联删除关联 session data
        db.Set<WebSessionEntity>()
            .Where(e => e.SessionId == sessionId)
            .ExecuteDelete();

        db.SaveChanges();
    }
}