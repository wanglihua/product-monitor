using Core.CodeType;

namespace Domain;

public record ClientKeyType : CodeTypeBase, ICodeType<ClientKeyType>
{
    /// <summary>
    /// 安装
    /// </summary>
    public static readonly ClientKeyType Install = new(100, "安装");

    /// <summary>
    /// 网址
    /// </summary>
    public static readonly ClientKeyType Url = new(200, "网址");

    /// <summary>
    /// None
    /// </summary>
    public static readonly ClientKeyType None = new(0, "");

    private ClientKeyType(int id, string name) : base(id, name)
    {
    }

    private static readonly IReadOnlyList<ClientKeyType> codeTypeList = new List<ClientKeyType>() {
        Install,
        Url,
    };

    public static IList<ClientKeyType> GetList()
    {
        return codeTypeList.ToList();
    }

    private static readonly IDictionary<int, ClientKeyType> codeTypeMap = codeTypeList.ToDictionary(e => e.Id);

    public static ClientKeyType GetById(int id)
    {
        if (codeTypeMap.TryGetValue(id, out var codeType))
        {
            return codeType;
        }

        return None;
    }
}