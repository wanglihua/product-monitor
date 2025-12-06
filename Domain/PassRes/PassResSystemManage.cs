namespace Domain.PassRes;

public partial class PassRes
{
    /// <summary>
    /// 系统管理
    /// </summary>
    public static class SystemManage
    {
        private const long CateId = PassResCate.SystemManageId;

        public static IList<PassResEntity> AllData => new List<PassResEntity>() {
            new() { Id = User, Name = "用户管理", CateId = CateId },
            new() { Id = Group, Name = "群组管理", CateId = CateId },
            new() { Id = PassRes, Name = "权限资源", CateId = CateId },
            new() { Id = ClientKey, Name = "客户端密钥", CateId = CateId },
            new() { Id = ActionLog, Name = "操作日志", CateId = CateId },
        };

        public static IList<long> All = AllData.Select(e => e.Id).ToList();

        /// <summary>
        /// 用户管理
        /// </summary>
        public const long User = CateId + 0200;

        /// <summary>
        /// 群组管理
        /// </summary>
        public const long Group = CateId + 0400;

        /// <summary>
        /// 权限资源
        /// </summary>
        public const long PassRes = CateId + 0600;

        /// <summary>
        /// 客户端密钥
        /// </summary>
        public const long ClientKey = CateId + 0700;

        /// <summary>
        /// 操作日志
        /// </summary>
        public const long ActionLog = CateId + 0800;
    }
}