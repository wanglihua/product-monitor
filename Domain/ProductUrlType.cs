using Core.CodeType;

namespace Domain;

/// <summary>
/// 网址类型
/// </summary>
public class ProductUrlType : CodeTypeBase, ICodeType<ProductUrlType>
{
    /// <summary>
    /// 试用网址
    /// </summary>
    public static readonly ProductUrlType Eval = new(1_00, "试用网址");

    /// <summary>
    /// 租赁网址
    /// </summary>
    public static readonly ProductUrlType Rent = new(2_00, "租赁网址");

    /// <summary>
    /// 网站网址
    /// </summary>
    public static readonly ProductUrlType Site = new(3_00, "网站网址");

    /// <summary>
    /// None
    /// </summary>
    public static readonly ProductUrlType None = new(0, "");

    private static readonly IReadOnlyList<ProductUrlType> codeTypeList = new List<ProductUrlType> {
        Eval,
        Rent,
        Site,
    };

    private static readonly IDictionary<int, ProductUrlType> codeTypeMap = codeTypeList.ToDictionary(e => e.Id);

    public static ProductUrlType GetById(int id)
    {
        if (codeTypeMap.TryGetValue(id, out var codeType))
        {
            return codeType;
        }

        return None;
    }

    public static IList<ProductUrlType> GetList()
    {
        return codeTypeList.ToList();
    }

    private ProductUrlType(int id, string name) : base(id, name)
    {
    }
}
