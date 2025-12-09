using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Client
{
    public static class PM
    {
        public static ClientType ClientType { get; private set; }
        public static string ClientKey { get; private set; }

        public static void Start(ClientType clientType, string clientKey)
        {
            ClientType = clientType;
            ClientKey = clientKey;

            // todo 开启定时任务
        }

        private static ConcurrentQueue<UseData> postInstallDataCollectQueue = new ConcurrentQueue<UseData>();

        /// <summary>
        /// 添加用户使用记录
        /// </summary>
        /// <param name="account">使用账号</param>
        /// <param name="accountType">账号类型, 管理员， 教师， 学生</param>
        public static void AddUse(string account, string accountType)
        {
            postInstallDataCollectQueue.Enqueue(new UseData {
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
                if (postInstallDataCollectQueue.IsEmpty == true)
                {
                    break;
                }

                if (postInstallDataCollectQueue.TryDequeue(out UseData useData) == true)
                {
                    result.Add(useData);
                }
            }

            return result;
        }
    }
}