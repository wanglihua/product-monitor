using Core.CodeType;

namespace Domain;

public record AccountType : CodeTypeBase, ICodeType<AccountType>
{
    /// <summary>
    /// 管理员
    /// </summary>
    public static readonly AccountType Admin = new(100, "管理员");

    /// <summary>
    /// 教师
    /// </summary>
    public static readonly AccountType Teacher = new(200, "教师");

    /// <summary>
    /// 学生
    /// </summary>
    public static readonly AccountType Student = new(300, "学生");

    /// <summary>
    /// None
    /// </summary>
    public static readonly AccountType None = new(0, "");


    private AccountType(int id, string name) : base(id, name)
    {
    }

    private static readonly IReadOnlyList<AccountType> codeTypeList = new List<AccountType> {
        Admin,
        Teacher,
        Student,
    };

    public static IList<AccountType> GetList()
    {
        return codeTypeList.ToList();
    }

    private static readonly IDictionary<int, AccountType> codeTypeMap = codeTypeList.ToDictionary(e => e.Id);

    public static AccountType GetById(int id)
    {
        if (codeTypeMap.TryGetValue(id, out var codeType))
        {
            return codeType;
        }

        return None;
    }
}