using Core;
using Core.Web;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Service;

[Service(ServiceType.Scoped)]
public class WebSessionTimeoutResetService
{
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

        db.Set<WebSessionEntity>()
            .Where(e => e.SessionId == sessionId)
            .ExecuteUpdate(e => e
                .SetProperty(x => x.UpdateTime, DateTime.Now)
            );

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