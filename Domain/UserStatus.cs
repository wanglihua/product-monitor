using Core.CodeType;

namespace Domain;

public class UserStatus : CodeTypeBase, ICodeType<UserStatus>
{
    /// <summary>
    /// 已启用
    /// </summary>
    public static readonly UserStatus Enabled = new(100, "已启用");

    /// <summary>
    /// 已禁用
    /// </summary>
    public static readonly UserStatus Disabled = new(200, "已禁用");

    /// <summary>
    /// None
    /// </summary>
    public static readonly UserStatus None = new(0, "");

    private static readonly IReadOnlyList<UserStatus> codeTypeList = new List<UserStatus>() {
        Enabled,
        Disabled,
    };

    public static IList<UserStatus> GetList()
    {
        return codeTypeList.ToList();
    }

    private static readonly IDictionary<int, UserStatus> codeTypeMap = codeTypeList.ToDictionary(e => e.Id);

    public static UserStatus GetById(int id)
    {
        if (codeTypeMap.TryGetValue(id, out var codeType))
        {
            return codeType;
        }

        return None;
    }

    private UserStatus(int id, string name) : base(id, name)
    {
    }
}

public class UserStatusJsonConverter : CodeTypeJsonConverter<UserStatus>
{
}