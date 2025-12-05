namespace Core.CodeType;

public record CodeTypeBase
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

    public override bool Equals(object obj)
    {
        Console.WriteLine("code type base equals");

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