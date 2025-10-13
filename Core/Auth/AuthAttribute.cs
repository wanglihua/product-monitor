namespace Core.Auth;

/// <summary>
/// 标记 Controller 或者 Action 的授权资源
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class AuthAttribute : Attribute
{
    /// <summary>
    /// 授权资源Id 列表， 如果值为 空 ，就表示不检查资源授权，只检查用户是否已经登录。
    /// PassResIdList 中只要有一个授权了，就是可以访问标注的 Controller 或 Action 了
    /// </summary>
    public IList<long> PassResIdList { get; set; }

    /// <summary>
    /// 标记只能是管理员才能访问的 Controller 或者 Action
    /// 对应菜单是否显示时，同样判断当前是否是管理员
    /// </summary>
    public bool AdminOnly { get; set; }

    private AuthAttribute(long passResId, bool adminOnly)
    {
        PassResIdList = new List<long>();
        if (passResId != 0)
        {
            PassResIdList.Add(passResId);
        }

        AdminOnly = adminOnly;
    }

    /// <summary>
    /// 这个构造函数标记的， 只是进行登录检查，用于 Admin Area 中。
    /// 但一般如果是在 Admin Area 中的，不加这个标记也会进行登录检查，
    /// 所以这个没有参数的构造函数加不加都一样
    /// </summary>
    public AuthAttribute() : this(0, false)
    {
    }

    public AuthAttribute(long passResId) : this(passResId, false)
    {
    }

    public AuthAttribute(bool adminOnly) : this(0, true)
    {
    }

    public AuthAttribute(params long[] passResIds)
    {
        PassResIdList = new List<long>();

        foreach (long passResId in passResIds)
        {
            if (passResId != 0)
            {
                PassResIdList.Add(passResId);
            }
        }

        AdminOnly = false;
    }
}