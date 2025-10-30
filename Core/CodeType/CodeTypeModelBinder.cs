using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Core.CodeType;

public abstract class CodeTypeModelBinder<T> : IModelBinder where T : CodeTypeBase, ICodeType<T>
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var modelName = bindingContext.ModelName;
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        var codeStr = valueProviderResult.FirstValue;
        T codeType = GetByCode(0); // 如果字符串为空就取默认的 None 值
        if (string.IsNullOrWhiteSpace(codeStr) == false && int.TryParse(codeStr.Trim(), out int code) == false)
        {
            codeType = GetByCode(code);
        }

        bindingContext.Result = ModelBindingResult.Success(codeType);
        return Task.CompletedTask;
    }

    private T GetByCode(int code)
    {
        // return CodeTypeUtil.GetByCode<T>(code);
        return T.GetByCode(code);
    }
}