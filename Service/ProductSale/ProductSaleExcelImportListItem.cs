namespace Service.ProductSale;

public class ProductSaleExcelImportListItem
{
    /// <summary>
    /// 学校
    /// </summary>
    public long CollegeId { get; set; }

    /// <summary>
    /// 产品
    /// </summary>
    public long ProductId { get; set; }

    /// <summary>
    /// 版本号
    /// </summary>
    public string Version { get; set; } = "";

    /// <summary>
    /// 销售时间
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// 服务到期时间
    /// </summary>
    public DateTime ServiceExpireDate { get; set; }

    /// <summary>
    /// 客户经理
    /// </summary>
    public string SalesManager { get; set; } = "";

    /// <summary>
    /// 联系老师
    /// </summary>
    public string ContactTeacher { get; set; } = "";

    /// <summary>
    /// 联系电话
    /// </summary>
    public string ContactMobile { get; set; } = "";

    /// <summary>
    /// 备注
    /// </summary>
    public string Description { get; set; } = "";
}