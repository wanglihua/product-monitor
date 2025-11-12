namespace Core.CodeType;

public interface ICodeType<T> where T : CodeTypeBase
{
    static abstract T GetById(int id);

    static abstract IList<T> GetList();
}