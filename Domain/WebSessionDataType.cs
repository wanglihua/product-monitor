using Core.CodeType;

namespace Domain;

public class WebSessionDataType : CodeTypeBase
{
    /// <summary>
    /// 登录用户Id
    /// </summary>
    public static readonly WebSessionDataType LoginUserId = new(100_00, "登录用户Id");

    /// <summary>
    /// None
    /// </summary>
    public static readonly WebSessionDataType None = new(0, "");

    private static readonly IReadOnlyList<WebSessionDataType> codeTypeList = new List<WebSessionDataType>() {
        LoginUserId,
    };

    private static readonly IDictionary<int, WebSessionDataType> codeTypeMap = codeTypeList.ToDictionary(e => e.Code);

    public static WebSessionDataType GetByCode(int code)
    {
        if (codeTypeMap.TryGetValue(code, out var codeType))
        {
            return codeType;
        }

        return None;
    }

    public static IList<WebSessionDataType> GetList()
    {
        return codeTypeList.ToList();
    }

    public WebSessionDataType(int code, string name) : base(code, name)
    {
    }
}

public class WebSessionDataTypeModelBinder : CodeTypeModelBinder<WebSessionDataType>
{
}

public class WebSessionDataTypeCodeJsonConverter : CodeTypeJsonConverter<WebSessionDataType> // 用法 [JsonConverter(typeof(SessionDataTypeCodeConverter))]
{
}

public class WebSessionDataTypeValueConverter : CodeTypeValueConverter<WebSessionDataType>
{
}