using Core.CodeType;

namespace Domain;

public class ActionLogType : CodeTypeBase
{
    /// <summary>
    /// 用户创建
    /// </summary>
    public static readonly ActionLogType UserCreate = new(100_000_00, "用户创建");

    /// <summary>
    /// None
    /// </summary>
    public static readonly ActionLogType None = new(0, "");

    private static readonly IReadOnlyList<ActionLogType> codeTypeList = new List<ActionLogType>() {
        UserCreate,
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

public class ActionLogTypeCodeJsonConverter : CodeTypeJsonConverter<ActionLogType> // 用法 [JsonConverter(typeof(SessionDataTypeCodeConverter))]
{
}

public class ActionLogTypeValueConverter : CodeTypeValueConverter<ActionLogType>
{
}