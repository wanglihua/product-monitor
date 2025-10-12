namespace Domain.PassRes;

public partial class PassRes
{
    /// <summary>
    /// 项目管理
    /// </summary>
    public static class ProjectManage
    {
        public const long CateId = PassResCate.ProjectManageId;

        public static IList<PassResEntity> AllData => new List<PassResEntity>() {
            new() { Id = ProductSale, Name = "销售目录", CateId = CateId },
            new() { Id = ProductRent, Name = "租赁目录", CateId = CateId },
            new() { Id = ProductEval, Name = "试用目录", CateId = CateId },
            new() { Id = ProjectAnalyze, Name = "项目分析", CateId = CateId },
        };

        public static IList<long> All = AllData.Select(e => e.Id).ToList();

        /// <summary>
        /// 销售目录
        /// </summary>
        public const long ProductSale = CateId + 0200;

        /// <summary>
        /// 租赁目录
        /// </summary>
        public const long ProductRent = CateId + 0400;

        /// <summary>
        /// 试用目录
        /// </summary>
        public const long ProductEval = CateId + 0600;

        /// <summary>
        /// 项目分析
        /// </summary>
        public const long ProjectAnalyze = CateId + 0800;
    }
}