using Core.CodeType;

namespace Domain;

/// <summary>
/// 院校办学层次类型
/// </summary>
public class CollegeLevelType : CodeTypeBase
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