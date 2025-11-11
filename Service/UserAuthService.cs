using Core;
using Domain;
using Microsoft.Extensions.Caching.Memory;

namespace Service;

[Service(ServiceType.Scoped)]
public class UserAuthService
{
    private static readonly string cacheKey = "UserAuthResult";

    // 不通过注入, 保持独立性, 到时定时任务中, 也可以设置这里的值
    private static IMemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());

    // 不同过注入, 保持独立, 到时定时任务中, 也可以同样操作
    private static readonly Db db = new Db();

    /// <summary>
    /// 从数据库授权记录, 建立授权缓存
    /// </summary>
    public void BuildAuthCache()
    {
    }

    /// <summary>
    /// 获取授权缓存列表
    /// </summary>
    public IList<AuthResult> GetAuthResultList()
    {
        return new List<AuthResult>();
    }

    /// <summary>
    /// 用户是否具有某个资源权限
    /// </summary>
    public bool UserHasPassRes(int userId, int passResId)
    {
        return false;
    }
}