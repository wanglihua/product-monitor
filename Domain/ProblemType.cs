using Core.CodeType;

namespace Domain;

/// <summary>
/// 问题类型
/// </summary>
public class ProblemType : CodeTypeBase, ICodeType<ProblemType>
{
    /// <summary>
    /// BUG(管理员端)
    /// </summary>
    public static readonly ProblemType BugAdmin = new(1_00, "BUG(管理员端)");

    /// <summary>
    /// BUG(教师端)
    /// </summary>
    public static readonly ProblemType BugTeacher = new(2_00, "BUG(教师端)");

    /// <summary>
    /// BUG(学生端)
    /// </summary>
    public static readonly ProblemType BugStudent = new(3_00, "BUG(学生端)");

    /// <summary>
    /// 服务器/安装
    /// </summary>
    public static readonly ProblemType ServerSetup = new(4_00, "服务器/安装");

    /// <summary>
    /// 背景资料/规则
    /// </summary>
    public static readonly ProblemType BackgroundRule = new(5_00, "背景资料/规则");

    /// <summary>
    /// 新需求
    /// </summary>
    public static readonly ProblemType NewRequirement = new(6_00, "新需求");

    /// <summary>
    /// 其他(技术类)
    /// </summary>
    public static readonly ProblemType OtherTech = new(7_00, "其他(技术类)");

    /// <summary>
    /// None
    /// </summary>
    public static readonly ProblemType None = new(0, "");


    private ProblemType(int id, string name) : base(id, name)
    {
    }

    private static readonly IReadOnlyList<ProblemType> codeTypeList = new List<ProblemType> {
        BugAdmin,
        BugTeacher,
        BugStudent,
        ServerSetup,
        BackgroundRule,
        NewRequirement,
        OtherTech,
    };

    private static readonly IDictionary<int, ProblemType> codeTypeMap = codeTypeList.ToDictionary(e => e.Id);

    public static ProblemType GetById(int id)
    {
        if (codeTypeMap.TryGetValue(id, out var codeType))
        {
            return codeType;
        }

        return None;
    }

    public static ProblemType GetByName(string name)
    {
        var list = GetList();

        foreach (var codeType in list)
        {
            if (codeType.Name == name)
            {
                return codeType;
            }
        }

        return None;
    }

    public static IList<ProblemType> GetList()
    {
        return codeTypeList.ToList();
    }
}

public class ProblemTypeJsonConverter : CodeTypeJsonConverter<ProblemType>
{
}

public class ProblemTypeModelBinder : CodeTypeModelBinder<ProblemType>
{
}