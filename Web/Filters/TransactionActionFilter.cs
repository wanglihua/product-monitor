using Core.Database;
using Domain;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Storage;
using NLog;

namespace Web.Filters;

/// <summary>
/// "services.AddScoped&lt;TransactionActionFilter&gt;();" // 必须是 AddScoped
/// </summary>
public class TransactionActionFilter : IActionFilter
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;

    private IDbContextTransaction? transaction;
    private bool autoTransaction = false;

    public TransactionActionFilter(Db db)
    {
        this.db = db;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
        if (actionDescriptor is null)
        {
            return;
        }

        object[] controllerAttributes = actionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(TransactionalAttribute), true);
        object[] actionAttributes = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(TransactionalAttribute), true);

        autoTransaction = false;
        if (controllerAttributes.Length != 0)
        {
            if (controllerAttributes[0] is TransactionalAttribute { Transactional: true })
            {
                autoTransaction = true;
            }
        }

        if (actionAttributes.Length != 0)
        {
            if (actionAttributes[0] is TransactionalAttribute { Transactional: true })
            {
                autoTransaction = true;
            }
        }

        if (autoTransaction)
        {
            transaction = db.Database.BeginTransaction();
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is not null)
        {
            if (context.Exception is Core.ValidationException == false)
            {
                log.Error(context.Exception.GetType());
                log.Error(context.Exception);
                log.Error(context.Exception.StackTrace ?? "");
            }

            if (autoTransaction == true)
            {
                transaction?.Rollback();
            }

            return;
        }

        try
        {
            if (context.Exception is null)
            {
                db.SaveChanges();
            }

            if (autoTransaction == true)
            {
                transaction?.Commit();
            }
        }
        catch (Exception exp)
        {
            if (exp is Core.ValidationException == false)
            {
                log.Error(exp);
            }

            if (autoTransaction == true)
            {
                try
                {
                    transaction?.Rollback();
                }
                catch (Exception expRollback)
                {
                    log.Error(exp);
                    log.Error(expRollback);

                    throw;
                }
            }

            throw;
        }
    }
}