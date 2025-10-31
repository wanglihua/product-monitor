using Core.CodeType;

namespace Domain;

public class WebSessionDataType : CodeTypeBase, ICodeType<WebSessionDataType>
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

    private static readonly IDictionary<int, WebSessionDataType> codeTypeMap = codeTypeList.ToDictionary(e => e.Id);

    public static WebSessionDataType GetById(int id)
    {
        if (codeTypeMap.TryGetValue(id, out var codeType))
        {
            return codeType;
        }

        return None;
    }

    public static IList<WebSessionDataType> GetList()
    {
        return codeTypeList.ToList();
    }

    private WebSessionDataType(int id, string name) : base(id, name)
    {
    }
}

public class WebSessionDataTypeModelBinder : CodeTypeModelBinder<WebSessionDataType>
{
}

public class WebSessionDataTypeCodeJsonConverter : CodeTypeJsonConverter<WebSessionDataType>
{
}

public class WebSessionDataTypeValueConverter : CodeTypeValueConverter<WebSessionDataType>
{
}