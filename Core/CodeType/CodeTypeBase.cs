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

    public static bool operator ==(CodeTypeBase? x, CodeTypeBase? y)
    {
        if (x is null && y is null)
        {
            return true;
        }

        if (x is not null)
        {
            return x.Equals(y);
        }

        if (y is not null)
        {
            return y.Equals(x);
        }

        // 这个不可能发生
        return false;
    }

    public static bool operator !=(CodeTypeBase? x, CodeTypeBase? y)
    {
        return !(x == y);
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