namespace Domain.PassRes;

public partial class PassRes
{
    /// <summary>
    /// 产品服务
    /// </summary>
    public static class ProductService
    {
        public const long CateId = PassResCate.ProductServiceId;

        public static IList<PassResEntity> AllData => new List<PassResEntity>() {
            new() { Id = ProductInstall, Name = "产品安装", CateId = CateId },
            new() { Id = ProductUpdate, Name = "产品更新", CateId = CateId },
            new() { Id = ProductSaleService, Name = "售后维护", CateId = CateId },
            new() { Id = ProductSpecialService, Name = "特殊售后", CateId = CateId },
            new() { Id = ProductServiceAnalyze, Name = "服务分析", CateId = CateId },
        };

        public static IList<long> All = AllData.Select(e => e.Id).ToList();

        /// <summary>
        /// 产品安装
        /// </summary>
        public const long ProductInstall = CateId + 0200;

        /// <summary>
        /// 产品更新
        /// </summary>
        public const long ProductUpdate = CateId + 0400;

        /// <summary>
        /// 售后维护
        /// </summary>
        public const long ProductSaleService = CateId + 0600; // 原本想取名 ProductService, 但跟 类名箱重, 暂时就用 ProductSaleService 了

        /// <summary>
        /// 特殊售后
        /// </summary>
        public const long ProductSpecialService = CateId + 0800;

        /// <summary>
        /// 服务分析
        /// </summary>
        public const long ProductServiceAnalyze = CateId + 1000;
    }
}