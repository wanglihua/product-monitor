namespace Core.CodeType;

public class CodeTypeBase
{
    /// <summary>
    /// 编码
    /// </summary>
    public int Id { get; }
    // public int Id { get; init; }
    // public int Id { get; protected set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; }
    // Fpublic string Name { get; init; }
    // public string Name { get; protected set; }

    protected CodeTypeBase(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }

    public override string ToString()
    {
        return Id.ToString();
    }

    public new bool Equals(object? x, object? y)
    {
        Console.WriteLine("code type base equals 1");

        if (x is null && y is null) // 两个同时为 null
        {
            return true;
        }

        if (x is null || y is null) // 两个中只有其中一个为 null
        {
            return false;
        }

        if ((x is CodeTypeBase xCodeType == false) || (y is CodeTypeBase yCodeType == false))
        {
            return false;
        }

        return xCodeType.Id == yCodeType.Id;
    }

    public override bool Equals(object? obj)
    {
        Console.WriteLine("code type base equals 2");

        if (obj is null)
        {
            return false;
        }

        if (obj is CodeTypeBase other == false)
        {
            return false;
        }

        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}