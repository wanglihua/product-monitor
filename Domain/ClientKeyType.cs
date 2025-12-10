using Core.CodeType;

namespace Domain;

public class ClientKeyType : CodeTypeBase, ICodeType<ClientKeyType>
{
    /// <summary>
    /// 产品安装
    /// </summary>
    public static readonly ClientKeyType Install = new(100, "产品安装");

    /// <summary>
    /// 试用网址
    /// </summary>
    public static readonly ClientKeyType UrlEval = new(200, "试用网址");

    /// <summary>
    /// 租赁网址
    /// </summary>
    public static readonly ClientKeyType UrlRent = new(300, "租赁网址");

    /// <summary>
    /// 网站网址
    /// </summary>
    public static readonly ClientKeyType UrlSite = new(400, "网站网址");

    /// <summary>
    /// None
    /// </summary>
    public static readonly ClientKeyType None = new(0, "");

    private ClientKeyType(int id, string name) : base(id, name)
    {
    }

    private static readonly IReadOnlyList<ClientKeyType> codeTypeList = new List<ClientKeyType>() {
        Install,
        UrlEval,
        UrlRent,
        UrlSite,
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