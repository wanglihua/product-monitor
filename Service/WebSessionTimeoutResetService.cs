using Core;
using Core.Web;
using Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class WebSessionTimeoutResetService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private static int runCount = 0;

    private readonly Db db;

    public WebSessionTimeoutResetService(Db db)
    {
        this.db = db;
    }

    public void ResetTimeout(string sessionId)
    {
        if (string.IsNullOrWhiteSpace(sessionId) == true)
        {
            return;
        }

        DateTime now = DateTime.Now;
        int count = db.Set<WebSessionEntity>()
            .Where(e => e.SessionId == sessionId)
            .ExecuteUpdate(e => e
                    .SetProperty(x => x.UpdateTime, now)
                    .SetProperty(x => x.UpdateCount, x => (x.UpdateCount + 1) % 100000) // 加了这个好像也没啥用
            );

        if (count == 0)
        {
            var webSession = new WebSessionEntity {
                SessionId = sessionId,
                UpdateCount = 0,
            };

            // 这里并发时, 有时会出问题
            try
            {
                db.Add<WebSessionEntity>(webSession);
                db.SaveChanges();
            }
            catch (DbUpdateException exp)
            {
                // 吃掉这个重复插入导致的唯一键异常, 暂时这么处理
                if (exp.InnerException is SqlException sqlEx && sqlEx.Message.Contains("ix_web_session_session_id"))
                {
                    db.ChangeTracker.TrackGraph(webSession, e => {
                        // 设置成分离的, 否则后面 SaveChanges() 的时候他还会试图保存
                        e.Entry.State = EntityState.Detached;
                    });
                    log.Error(exp);
                }
                else
                {
                    throw;
                }
            }
        }

        // 做 web session 的清理
        if (runCount % 10000 == 0) // runCount 初始化为0，保证第一次能得到清理，后面10000次清理一次
        {
            // 多加了10分钟是让过期10分钟的数据也保留了
            var timeoutSessionUpdateTime = DateTime.Now.Add(-WebSessionService.SessionTimeout.Add(TimeSpan.FromMinutes(10)));
            // 外键 ON DELETE CASCADE 存在，会同时清理 WebSessionDataEntity
            db.Set<WebSessionEntity>()
                .Where(e => e.UpdateTime < timeoutSessionUpdateTime)
                .ExecuteDelete();
        }

        runCount++;
    }
}