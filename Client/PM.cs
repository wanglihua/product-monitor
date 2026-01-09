using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Quartz.Impl;

namespace Client
{
    public static class PM
    {
#if DEBUG
        public static string PMSiteBase = "http://localhost:5001";
#else
        public static string PMSiteBase = "http://47.120.44.124:9006";
#endif

        public static ClientType ClientType { get; private set; }
        public static string ClientKey { get; private set; }

        /// <summary>
        /// 在 asp.net core 的 app.Run() 前调用，或者在 asp.net framework 的 Application_Start() 中调用
        /// </summary>
        /// <param name="clientType"></param>
        /// <param name="clientKey"></param>
        public static void Start(ClientType clientType, string clientKey)
        {
            // 检查 client key
            if (ClientKeyUtil.ValidateKey(clientKey) == false)
            {
                throw new Exception("不合法的 PM client key");
            }

            ClientType = clientType;
            ClientKey = clientKey;

            // 开启定时任务
            var scheduler = StdSchedulerFactory.GetDefaultScheduler().GetAwaiter().GetResult();
            scheduler.Start().Wait();
            DataUploadJob.Schedule(scheduler);
        }

        private static ConcurrentQueue<UseData> useDataCollectQueue = new ConcurrentQueue<UseData>();

        /// <summary>
        /// 添加用户使用记录
        /// </summary>
        /// <param name="account">使用账号</param>
        /// <param name="accountType">账号类型, 管理员， 教师， 学生</param>
        public static void AddUse(string account, string accountType)
        {
            useDataCollectQueue.Enqueue(new UseData {
                Account = account,
                AccountType = accountType,
                UseTime = DateTime.Now,
            });
        }

        public static List<UseData> DequeueData(int count)
        {
            var result = new List<UseData>();

            if (count <= 0)
            {
                return result;
            }

            for (int index = 0; index < count; index++)
            {
                if (useDataCollectQueue.IsEmpty == true)
                {
                    break;
                }

                if (useDataCollectQueue.TryDequeue(out UseData useData) == true)
                {
                    result.Add(useData);
                }
            }

            return result;
        }
    }
}