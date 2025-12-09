using System.Collections.Concurrent;

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

        public static ConcurrentQueue<UseData> PostInstallDataCollectQueue = new ConcurrentQueue<UseData>();

        public static void AddUse(string account, string accountType)
        {
        }
    }
}