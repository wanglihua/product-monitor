namespace Domain.PassRes;

public partial class PassRes
{
    /// <summary>
    /// 数据监测
    /// </summary>
    public static class DataMonitor
    {
        private const long CateId = PassResCate.DataMonitorId;

        public static IList<PassResEntity> AllData => new List<PassResEntity>() {
            new() { Id = ProductInstallMonitor, Name = "安装监测", CateId = CateId },
            new() { Id = ProductUrlMonitor, Name = "网站监测", CateId = CateId },
            new() { Id = ExceptionMonitor, Name = "异常监测", CateId = CateId },
        };

        public static IList<long> All = AllData.Select(e => e.Id).ToList();

        /// <summary>
        /// 安装监测
        /// </summary>
        public const long ProductInstallMonitor = CateId + 0200;

        /// <summary>
        /// 网站监测
        /// </summary>
        public const long ProductUrlMonitor = CateId + 0400;

        /// <summary>
        /// 异常监测
        /// </summary>
        public const long ExceptionMonitor = CateId + 0600;
    }
}