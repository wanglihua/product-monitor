using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Core.CodeType;

public class CodeTypeConvertions<T> : ValueConverter<T, int> where T : CodeTypeBase, ICodeType<T>
{
    private static T GetById(int id)
    {
        return T.GetById(id);
    }

    public CodeTypeConvertions() : base(codeType => codeType.Id, code => GetById(code))
    {
    }
}