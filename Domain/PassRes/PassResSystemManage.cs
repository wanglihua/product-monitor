namespace Domain.PassRes;

public partial class PassRes
{
    /// <summary>
    /// 系统管理
    /// </summary>
    public static class SystemManage
    {
        public const long CateId = PassResCate.SystemManageId;

        public static IList<PassResEntity> AllData => new List<PassResEntity>() {
            new() { Id = ActionLog, Name = "操作日志", CateId = CateId },
        };

        public static IList<long> All = AllData.Select(e => e.Id).ToList();

        /// <summary>
        /// 操作日志
        /// </summary>
        public const long ActionLog = CateId + 0200;
    }
}