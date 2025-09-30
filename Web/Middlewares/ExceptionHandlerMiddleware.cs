using Core;
using NLog;

namespace Web.Middlewares;

/// <summary>
/// 与 ExceptionFilter 合起来使用，捕捉所有异常
/// </summary>
public class ExceptionHandlerMiddleware
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly RequestDelegate next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception exp)
        {
            if ((exp is ValidationException) == false)
            {
                log.Error(exp);
            }

            if (exp.InnerException != null)
            {
                if ((exp.InnerException is ValidationException) == false)
                {
                    log.Error(exp.InnerException);
                }

                throw exp.InnerException;
            }

            throw;
        }
    }
}