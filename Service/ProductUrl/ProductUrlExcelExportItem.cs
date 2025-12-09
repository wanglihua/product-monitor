namespace Service.ProductUrl;

public class ProductUrlExcelExportItem
{
    public long Id { get; set; }

    /// <summary>
    /// 产品
    /// </summary>
    public string ProductName { get; set; } = "";

    /// <summary>
    /// 网址
    /// </summary>
    public string Url { get; set; } = "";

    /// <summary>
    /// 类型
    /// </summary>
    public string Type { get; set; } = "";

    /// <summary>
    /// 账号
    /// </summary>
    public string LoginUser { get; set; } = "";

    /// <summary>
    /// 密码
    /// </summary>
    public string LoginPassword { get; set; } = "";

    /// <summary>
    /// 备注
    /// </summary>
    public string Description { get; set; } = "";
}