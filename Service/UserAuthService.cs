using Core;
using Domain;
using Microsoft.Extensions.Caching.Memory;

namespace Service;

[Service(ServiceType.Scoped)]
public class UserAuthService(Db db)
{
    private static string cacheKey => "UserAuthResult";

    // 不通过注入, 保持独立性, 到时定时任务中, 也可以设置这里的值
    private static IMemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());

    /// <summary>
    /// 从数据库授权记录, 建立授权缓存
    /// </summary>
    public IList<UserPassRes> BuildAuthCache()
    {
        var userPassResList = GetUserPassResList();
        memoryCache.Set(cacheKey, userPassResList);
        return userPassResList;
    }

    private IList<UserPassRes> GetUserPassResList()
    {
        var userPassResEntityList = db.UserPassRes.ToList();

        var groupPassResList = db.GroupUser
            .Join(db.GroupPassRes,
                gu => gu.GroupId,
                gpr => gpr.GroupId,
                (gu, gpr) => new { GroupUser = gu, GroupPassRes = gpr })
            .ToList();

        var userPassResList = new List<UserPassRes>();
        foreach (var userPassResEntity in userPassResEntityList)
        {
            userPassResList.Add(new UserPassRes {
                UserId = userPassResEntity.UserId,
                PassResId = userPassResEntity.PassResId,
            });
        }

        foreach (var groupPassRes in groupPassResList)
        {
            userPassResList.Add(new UserPassRes {
                UserId = groupPassRes.GroupUser.UserId,
                PassResId = groupPassRes.GroupPassRes.PassResId,
            });
        }

        return userPassResList;
    }

    /// <summary>
    /// 获取授权缓存列表
    /// </summary>
    public IList<UserPassRes> GetAuthResultList()
    {
        if (memoryCache.TryGetValue(cacheKey, out IList<UserPassRes>? userPassResList) == false)
        {
            userPassResList = BuildAuthCache();
        }

        return userPassResList!;
    }

    /// <summary>
    /// 用户是否具有某个资源权限
    /// 这里考虑了管理员
    /// </summary>
    public bool UserHasPassRes(long userId, long passResId, bool isAdmin)
    {
        if (isAdmin == true)
        {
            return true;
        }

        return UserHasPassRes(userId, passResId);
    }

    /// <summary>
    /// 用户是否具有某个资源权限
    /// 这里不考虑管理员
    /// </summary>
    public bool UserHasPassRes(long userId, long passResId)
    {
        if (memoryCache.TryGetValue(cacheKey, out IList<UserPassRes>? userPassResList) == false)
        {
            userPassResList = BuildAuthCache();
        }

        var userPassRes = userPassResList!
            .Where(e => e.UserId == userId && e.PassResId == passResId)
            .Take(1)
            .SingleOrDefault();

        return userPassRes != null;
    }

    /// <summary>
    /// 用户是否具有某个资源权限列表中某个权限
    /// 这里考虑了管理员
    /// </summary>
    public bool UserHasOneOfPassRes(long userId, IList<long> passResIdList, bool isAdmin)
    {
        if (isAdmin == true)
        {
            return true;
        }

        return UserHasOneOfPassRes(userId, passResIdList);
    }

    /// <summary>
    /// 用户是否具有某个资源权限列表中某个权限
    /// 这里不考虑管理员
    /// </summary>
    public bool UserHasOneOfPassRes(long userId, IList<long> passResIdList)
    {
        if (passResIdList == null || passResIdList.Count == 0)
        {
            return false;
        }

        if (memoryCache.TryGetValue(cacheKey, out IList<UserPassRes>? userPassResList) == false)
        {
            userPassResList = BuildAuthCache();
        }

        var userPassRes = userPassResList!
            .Where(e => e.UserId == userId && passResIdList.Contains(e.PassResId))
            .Take(1)
            .SingleOrDefault();

        return userPassRes != null;
    }
}