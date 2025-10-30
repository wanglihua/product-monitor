using Core.CodeType;

namespace Domain;

/// <summary>
/// 院校办学层次类型
/// </summary>
public class CollegeLevelType : CodeTypeBase, ICodeType<CollegeLevelType>
{
    /// <summary>
    /// 本科
    /// </summary>
    public static readonly CollegeLevelType Undergraduate = new(1, "本科");

    /// <summary>
    /// 专科
    /// </summary>
    public static readonly CollegeLevelType JuniorCollege = new(2, "专科");

    /// <summary>
    /// 中职
    /// </summary>
    public static readonly CollegeLevelType VocationalSchool = new(3, "中职");

    /// <summary>
    /// None
    /// </summary>
    public static readonly CollegeLevelType None = new(0, "");

    private static readonly IReadOnlyList<CollegeLevelType> codeTypeList = new List<CollegeLevelType>() {
        Undergraduate,
        JuniorCollege,
        VocationalSchool,
    };

    public static IList<CollegeLevelType> GetList()
    {
        return codeTypeList.ToList();
    }

    private static readonly IDictionary<int, CollegeLevelType> codeTypeMap = codeTypeList.ToDictionary(e => e.Code);

    public static CollegeLevelType GetByCode(int code)
    {
        if (codeTypeMap.TryGetValue(code, out var codeType))
        {
            return codeType;
        }

        return None;
    }

    private CollegeLevelType(int code, string name) : base(code, name)
    {
    }
}

public class CollegeLevelTypeModelBinder : CodeTypeModelBinder<CollegeLevelType>
{
}

public class CollegeLevelTypeCodeJsonConverter : CodeTypeJsonConverter<CollegeLevelType>
{
}

public class CollegeLevelTypeValueConverter : CodeTypeValueConverter<CollegeLevelType>
{
}