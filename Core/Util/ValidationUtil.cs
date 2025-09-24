using System.Text;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Core.Util;

public static class ValidationUtil
{
    public static string GetErrorMessages(ModelStateDictionary modelState)
    {
        StringBuilder sb = new StringBuilder();

        foreach (KeyValuePair<string, ModelStateEntry> entry in modelState)
        {
            foreach (ModelError modelError in entry.Value.Errors)
            {
                sb.AppendFormat("{0}<br />{1}", modelError.ErrorMessage, Environment.NewLine);
            }
        }

        return sb.ToString();
    }

    public static string GetErrorMessagesForMobile(ModelStateDictionary modelState)
    {
        StringBuilder sb = new StringBuilder();

        foreach (KeyValuePair<string, ModelStateEntry> entry in modelState)
        {
            foreach (ModelError modelError in entry.Value.Errors)
            {
                sb.AppendFormat("{0}{1}", modelError.ErrorMessage, Environment.NewLine);
            }
        }

        return sb.ToString();
    }

    public static string GetErrorMessages(IList<string> messageList)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var message in messageList)
        {
            sb.AppendFormat("{0}<br />{1}", message, Environment.NewLine);
        }

        return sb.ToString();
    }

    public static string GetErrorMessagesForMobile(IList<string> messageList)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var message in messageList)
        {
            sb.AppendFormat("{0}{1}", message, Environment.NewLine);
        }

        return sb.ToString();
    }

    public static void Validate<V, E>(E data) where V : AbstractValidator<E>, new()
    {
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

    public static void ValidateEntity<V, E>(E data) where V : AbstractValidator<E>, new()
    {
        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

        var validator = new V();
        ValidationResult result = validator.Validate(data);
        if (result.IsValid == false)
        {
            // 只提示第一个错误
            string message = result.Errors.FirstOrDefault()?.ErrorMessage ?? string.Empty;
            // throw new ValidationFailure(message); 暂时也抛出 ValidationException 
            throw new ValidationException(message);
        }
    }
}