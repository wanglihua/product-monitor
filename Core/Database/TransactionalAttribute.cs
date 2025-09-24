namespace Core.Database;

/// <summary>
/// 用来标记某个Action是不是启用事务
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class TransactionalAttribute : Attribute
{
    public bool Transactional { get; set; }

    public TransactionalAttribute() : this(true)
    {
    }

    public TransactionalAttribute(bool transactional)
    {
        this.Transactional = transactional;
    }
}