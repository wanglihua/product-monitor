using Core.CodeType;

namespace Domain;

public class ProductUrlType : CodeTypeBase
{
    /// <summary>
    /// 试用网址
    /// </summary>
    public static readonly ProductUrlType Evaluating = new(1_00, "试用网址");

    /// <summary>
    /// 租赁网址
    /// </summary>
    public static readonly ProductUrlType Rent = new(2_00, "租赁网址");

    /// <summary>
    /// 网站网址
    /// </summary>
    public static readonly ProductUrlType WebSite = new(3_00, "网站网址");

    /// <summary>
    /// None
    /// </summary>
    public static readonly ProductUrlType None = new(0, "");

    private static readonly IReadOnlyList<ProductUrlType> codeTypeList = new List<ProductUrlType> {
        Evaluating,
        Rent,
        WebSite,
    };

    private static readonly IDictionary<int, ProductUrlType> codeTypeMap = codeTypeList.ToDictionary(e => e.Code);

    public static ProductUrlType GetByCode(int code)
    {
        if (codeTypeMap.TryGetValue(code, out var codeType))
        {
            return codeType;
        }

        return None;
    }

    public static IList<ProductUrlType> GetList()
    {
        return codeTypeList.ToList();
    }

    public ProductUrlType(int code, string name) : base(code, name)
    {
    }
}

public class ProductUrlTypeModelBinder : CodeTypeModelBinder<ProductUrlType>
{
}

public class ProductUrlTypeCodeJsonConverter : CodeTypeJsonConverter<ProductUrlType>
{
}

public class ProductUrlTypeValueConverter : CodeTypeValueConverter<ProductUrlType>
{
}