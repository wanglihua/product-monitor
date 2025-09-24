namespace Core.CodeType;

public abstract class CodeTypeBase
{
    /// <summary>
    /// 编码
    /// </summary>
    public int Code { get; protected set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; protected set; }

    protected CodeTypeBase(int code, string name)
    {
        this.Code = code;
        this.Name = name;
    }

    public override string ToString()
    {
        return Code.ToString();
    }

    public new bool Equals(object? x, object? y)
    {
        if (x == null && y == null) // 两个同时为 null
        {
            return true;
        }

        if (x == null || y == null) // 两个中只有其中一个为 null
        {
            return false;
        }

        CodeTypeBase? xCodeType = x as CodeTypeBase;
        CodeTypeBase? yCodeType = y as CodeTypeBase;

        if (xCodeType == null || yCodeType == null)
        {
            return false;
        }

        return xCodeType.Code == yCodeType.Code;
    }
}