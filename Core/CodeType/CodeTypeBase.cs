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

    /// <summary>
    /// 判断相等, 这个的实现方式很重要
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public new bool Equals(object? x, object? y)
    {
        Console.WriteLine("code type base equals");
        
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
}