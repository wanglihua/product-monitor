namespace Domain.PassRes;

public partial class PassRes
{
    /// <summary>
    /// 基础设置
    /// </summary>
    public static class BasicSetting
    {
        private const long CateId = PassResCate.BasicSettingId;

        public static IList<PassResEntity> AllData => new List<PassResEntity>() {
            new() { Id = Product, Name = "产品目录", CateId = CateId },
            new() { Id = College, Name = "院校目录", CateId = CateId },
            new() { Id = ProductUrl, Name = "网址管理", CateId = CateId },
        };

        public static IList<long> All = AllData.Select(e => e.Id).ToList();

        /// <summary>
        /// 产品目录
        /// </summary>
        public const long Product = CateId + 0200;

        /// <summary>
        /// 院校目录
        /// </summary>
        public const long College = CateId + 0400;

        /// <summary>
        /// 网址管理
        /// </summary>
        public const long ProductUrl = CateId + 0600;
    }
}