using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Core.CodeType;

public abstract class CodeTypeValueConverter<T> : ValueConverter<T, int> where T : CodeTypeBase, ICodeType<T>
{
    private static T GetById(int id)
    {
        // return CodeTypeUtil.GetByCode<T>(id);
        return T.GetById(id);
    }

    protected CodeTypeValueConverter() : base(codeType => codeType.Id, code => GetById(code))
    {
    }
}