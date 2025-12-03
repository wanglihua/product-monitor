using Core.CodeType;

namespace Domain;

/// <summary>
/// 项目类型
/// </summary>
public record ProjectType : CodeTypeBase, ICodeType<ProjectType>
{
    /// <summary>
    /// 销售项目
    /// </summary>
    public static readonly ProjectType Sale = new(1_00, "销售项目");

    /// <summary>
    /// 租赁项目
    /// </summary>
    public static readonly ProjectType Rent = new(2_00, "租赁项目");

    /// <summary>
    /// 试用项目
    /// </summary>
    public static readonly ProjectType Eval = new(3_00, "试用项目");

    /// <summary>
    /// None
    /// </summary>
    public static readonly ProjectType None = new(0, "");

    public static readonly IReadOnlyList<ProjectType> codeTypeList = new List<ProjectType> {
        Sale,
        Rent,
        Eval,
    };

    public static readonly IDictionary<int, ProjectType> codeTypeMap = codeTypeList.ToDictionary(e => e.Id);

    public static ProjectType GetById(int id)
    {
        if (codeTypeMap.TryGetValue(id, out var codeType))
        {
            return codeType;
        }

        return None;
    }

    public static IList<ProjectType> GetList()
    {
        return codeTypeList.ToList();
    }

    private ProjectType(int id, string name) : base(id, name)
    {
    }
}