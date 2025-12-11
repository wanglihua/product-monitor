namespace Service.ProductRent;

public class ProductRentExcelImportItem
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
    /// 开始时间
    /// </summary>
    public DateTime BeginDate { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndDate { get; set; }

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