using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Core.CodeType;

public abstract class CodeTypeValueConverter<T> : ValueConverter<T, int> where T : CodeTypeBase
{
    private static T GetByCode(int code)
    {
        return CodeTypeUtil.GetByCode<T>(code);
    }

    protected CodeTypeValueConverter() : base(codeType => codeType.Code, code => GetByCode(code))
    {
    }
}