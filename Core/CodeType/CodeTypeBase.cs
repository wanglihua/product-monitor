namespace Core.CodeType;

public record CodeTypeBase
{
    /// <summary>
    /// 编码
    /// </summary>
    public int Id { get; protected set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; protected set; }

    protected CodeTypeBase(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }
}