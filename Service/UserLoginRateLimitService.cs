using Core;
using Microsoft.Extensions.Caching.Memory;

namespace Service;

/// <summary>
/// 用户登录频率限制，不能过于频繁
/// </summary>
[Service(ServiceType.Scoped)]
public class UserLoginRateLimitService(IMemoryCache memoryCache)
{
    /// <summary>
    /// 限制时间60秒
    /// </summary>
    private static readonly int limitSeconds = 60;

    /// <summary>
    /// 在限制时间内，可以登录的次数
    /// </summary>
    private static readonly int limitCount = 5;

    public void CheckRateLimit(string loginName)
    {
        if (string.IsNullOrWhiteSpace(loginName))
        {
            throw new ValidationException("登录名不能为空！");
        }

        string key = "user_login_rate_count_" + loginName;
        if (memoryCache.TryGetValue(key, out IList<DateTime>? loginTimeList) == false)
        {
            loginTimeList = new List<DateTime>();
        }

        // 检查值
        DateTime now = DateTime.Now;

        // 存储当前尝试登录时间
        // 无论是否下面报出异常，这里都记录时间
        loginTimeList!.Add(now);

        int loginCount = loginTimeList.Count(e => e >= now.AddSeconds(-limitSeconds)); // 最近有缓存记录的登录次数
        // 60 秒内最多登录次数不能大于 5 次
        if (loginCount > limitCount)
        {
            throw new ValidationException("当前用户登录过于频繁，请稍等再重试！");
        }

        memoryCache.Set(key, loginTimeList, TimeSpan.FromSeconds(limitSeconds * 10)); // 缓存保留10分钟
    }
}