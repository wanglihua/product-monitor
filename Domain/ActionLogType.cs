using Core.CodeType;

namespace Domain;

public class ActionLogType : CodeTypeBase
{
    /// <summary>
    /// 用户登录
    /// </summary>
    public static readonly ActionLogType UserLogin = new(0002_0000, "用户登录");

    /// <summary>
    /// 用户创建
    /// </summary>
    public static readonly ActionLogType UserCreate = new(0004_0000, "用户创建");

    /// <summary>
    /// 用户更新
    /// </summary>
    public static readonly ActionLogType UserUpdate = new(0006_0000, "用户更新");

    /// <summary>
    /// 用户删除
    /// </summary>
    public static readonly ActionLogType UserDelete = new(0008_0000, "用户删除");

    /// <summary>
    /// 管理员添加
    /// </summary>
    public static readonly ActionLogType AdminUserAdd = new(0010_0000, "管理员添加");

    /// <summary>
    /// 管理员移除
    /// </summary>
    public static readonly ActionLogType AdminUserRemove = new(0012_0000, "管理员移除");
    
    /// <summary>
    /// 权限资源数据更新
    /// </summary>
    public static readonly ActionLogType PassResDataInit = new(0014_0000, "权限资源数据更新");
    
    /// <summary>
    /// 产品目录创建
    /// </summary>
    public static readonly ActionLogType ProductCreate = new(0016_0000, "产品目录创建");

    /// <summary>
    /// 产品目录更新
    /// </summary>
    public static readonly ActionLogType ProductUpdate = new(0018_0000, "产品目录更新");

    /// <summary>
    /// 产品目录删除
    /// </summary>
    public static readonly ActionLogType ProductDelete = new(0020_0000, "产品目录删除");

    /// <summary>
    /// None
    /// </summary>
    public static readonly ActionLogType None = new(0, "");

    private static readonly IReadOnlyList<ActionLogType> codeTypeList = new List<ActionLogType>() {
        UserLogin,
        UserCreate,
        UserUpdate,
        UserDelete,
        AdminUserAdd,
        AdminUserRemove,
        PassResDataInit,
        ProductCreate,
        ProductUpdate,
        ProductDelete,
    };

    private static readonly IDictionary<int, ActionLogType> codeTypeMap = codeTypeList.ToDictionary(e => e.Code);

    public static ActionLogType GetByCode(int code)
    {
        if (codeTypeMap.TryGetValue(code, out var codeType))
        {
            return codeType;
        }

        return None;
    }

    public static IList<ActionLogType> GetList()
    {
        return codeTypeList.ToList();
    }

    public ActionLogType(int code, string name) : base(code, name)
    {
    }
}

public class ActionLogTypeModelBinder : CodeTypeModelBinder<ActionLogType>
{
}

public class ActionLogTypeCodeJsonConverter : CodeTypeJsonConverter<ActionLogType>
{
}

public class ActionLogTypeValueConverter : CodeTypeValueConverter<ActionLogType>
{
}