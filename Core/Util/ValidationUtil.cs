using FluentValidation;
using FluentValidation.Results;

namespace Core.Util;

public static class ValidationUtil
{
    public static void Validate<V, E>(E data) where V : AbstractValidator<E>, new()
    {
        if (data == null)
        {
            throw new ValidationException("未能正确接收客户端数据, 请确认数据格式是否匹配!");
        }

        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

        var validator = new V();
        ValidationResult result = validator.Validate(data);
        if (result.IsValid == false)
        {
            // 只提示第一个错误
            string message = result.Errors.FirstOrDefault()?.ErrorMessage ?? string.Empty;
            throw new ValidationException(message);
        }
    }
}