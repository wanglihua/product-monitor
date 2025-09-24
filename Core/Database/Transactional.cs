using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NLog;

namespace Core.Database;

public static class Transactional
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// 用法
    /// Transactional.Wrap(db, () => {
    ///     // 这里写原来代码
    /// });
    /// </summary>
    public static void Wrap(DbContext db, Action action)
    {
        bool transactionBegin = false;

        IDbContextTransaction? transaction = null;

        try
        {
            transaction = db.Database.CurrentTransaction;
            if (transaction == null)
            {
                transaction = db.Database.BeginTransaction();
                transactionBegin = true;
            }

            // 执行业务代码
            action();

            db.SaveChanges(); // 这个是不是这里一定要调用呢

            if (transactionBegin == true)
            {
                transaction.Commit();
            }
        }
        catch (Exception exp)
        {
            if (exp is ValidationException == false)
            {
                log.Error(exp);
            }

            if (transactionBegin == true)
            {
                transaction?.Rollback();
            }

            throw;
        }
    }

    public static void WrapWithLock(object lockObj, DbContext db, Action action)
    {
        lock (lockObj)
        {
            Wrap(db, action);
        }
    }

    public delegate T TransactionAction<out T>();

    /// <summary>
    /// 用法
    /// var result = Transactional.Wrap(db, () => {
    ///     // 这里写原来代码
    ///     return someValue;
    /// });
    /// </summary>
    public static T Wrap<T>(DbContext db, TransactionAction<T> action)
    {
        bool transactionBegin = false;

        IDbContextTransaction? transaction = null;

        try
        {
            transaction = db.Database.CurrentTransaction;
            if (transaction == null)
            {
                transaction = db.Database.BeginTransaction();
                transactionBegin = true;
            }

            // 执行业务代码
            T t = action();

            db.SaveChanges(); // 这个是不是这里一定要调用呢

            if (transactionBegin == true)
            {
                transaction.Commit();
            }

            return t;
        }
        catch (Exception exp)
        {
            if (exp is ValidationException == false)
            {
                log.Error(exp);
            }

            if (transactionBegin == true)
            {
                transaction?.Rollback();
            }

            throw;
        }
    }

    public static T WrapWithLock<T>(object lockObj, DbContext db, TransactionAction<T> action)
    {
        lock (lockObj)
        {
            return Wrap(db, action);
        }
    }
}