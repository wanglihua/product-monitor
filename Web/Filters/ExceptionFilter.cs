using Core.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NLog;
using Web.Models.Error;

namespace Web.Filters;

/// <summary>
/// 与 ExceptionHandlerMiddleware 合起来使用，捕捉所有异常
/// </summary>
public class ExceptionFilter : IExceptionFilter
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    public void OnException(ExceptionContext context)
    {
        var exp = context.Exception;

        if (exp == null)
        {
            return;
        }

        string message = exp.Message;

        var request = context.HttpContext.Request;

        if (WebUtil.ShouldShowJson(request))
        {
            context.Result = new JsonResult(new Dictionary<string, object>() {
                { "success", false },
                { "message", message }
            });
        }
        else
        {
            context.Result = new ViewResult() {
                ViewName = "Error/ErrorIndex",
                ViewData = new ViewDataDictionary(
                    new EmptyModelMetadataProvider(),
                    new ModelStateDictionary()
                ) {
                    Model = new ErrorViewModel {
                        Message = message,
                    },
                }
            };
        }
    }
}