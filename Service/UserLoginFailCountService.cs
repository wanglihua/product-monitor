using Core;
using Microsoft.Extensions.Caching.Memory;

namespace Service;

[Service(ServiceType.Scoped)]
public class UserLoginFailCountService(IMemoryCache memoryCache)
{
    /// <summary>
    /// 限制时间60分钟
    /// </summary>
    private static readonly int limitMinutes = 60;

    /// <summary>
    /// 在限制时间内，可以不用验证码登录失败的次数
    /// </summary>
    private static readonly int limitCount = 3;

    /// <summary>
    /// 添加一次失败记录, 并返回是否应该显示验证码
    /// </summary>
    public bool Add(string loginName)
    {
        if (string.IsNullOrWhiteSpace(loginName))
        {
            throw new ValidationException("登录名不能为空！");
        }

        string key = GetKey(loginName);
        if (memoryCache.TryGetValue(key, out IList<DateTime>? failTimeList) == false)
        {
            failTimeList = new List<DateTime>();
        }

        if (failTimeList is null)
        {
            failTimeList = new List<DateTime>();
        }

        DateTime now = DateTime.Now;

        failTimeList.Add(now);

        memoryCache.Set(key, failTimeList, TimeSpan.FromMinutes(limitMinutes * 2)); // 缓存保留2个小时应该差不多

        int failCount = failTimeList.Count(e => e >= now.AddMinutes(-limitMinutes));
        if (failCount <= limitCount)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 是否应该显示验证码
    /// </summary>
    public bool ShouldShowCaptcha(string loginName)
    {
        if (string.IsNullOrWhiteSpace(loginName))
        {
            throw new ValidationException("登录名不能为空！");
        }

        string key = GetKey(loginName);
        if (memoryCache.TryGetValue(key, out IList<DateTime>? failTimeList) == false)
        {
            return false;
        }

        if (failTimeList is null)
        {
            return false;
        }

        DateTime now = DateTime.Now;

        int failCount = failTimeList.Count(e => e >= now.AddMinutes(-limitMinutes));
        if (failCount <= limitCount)
        {
            return false;
        }

        return true;
    }

    private string GetKey(string loginName)
    {
        return $"user_login_fail_count_{loginName}";
    }
}