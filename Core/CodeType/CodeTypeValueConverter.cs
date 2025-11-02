using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Core.CodeType;

public class CodeTypeValueConverter<T> : ValueConverter<T, int> where T : CodeTypeBase, ICodeType<T>
{
    private static T GetById(int id)
    {
        return T.GetById(id);
    }

    public CodeTypeValueConverter() : base(codeType => codeType.Id, code => GetById(code))
    {
    }
}