using Microsoft.CodeAnalysis.FlowAnalysis;

namespace Core.CodeType;

public interface ICodeType<T> where T : CodeTypeBase
{
    static abstract T GetByCode(int code);

    static abstract IList<T> GetList();
}