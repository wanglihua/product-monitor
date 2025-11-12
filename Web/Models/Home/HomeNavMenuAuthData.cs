using Newtonsoft.Json;

namespace Web.Models.Home;

public class HomeNavMenuAuthData
{
    /// <summary>
    /// 是不是管理员
    /// </summary>
    [JsonProperty("admin")]
    public bool Admin { get; set; }

    /// <summary>
    /// 是不是这个目录要显示
    /// </summary>
    [JsonProperty("dataMonitor")]
    public bool DataMonitor { get; set; }

    [JsonProperty("productInstallMonitor")]
    public bool ProductInstallMonitor { get; set; }

    [JsonProperty("productUrlMonitor")]
    public bool ProductUrlMonitor { get; set; }

    [JsonProperty("exceptionMonitor")]
    public bool ExceptionMonitor { get; set; }

    /// <summary>
    /// 是不是这个目录要显示
    /// </summary>
    [JsonProperty("productService")]
    public bool ProductService { get; set; }

    [JsonProperty("productInstall")]
    public bool ProductInstall { get; set; }

    [JsonProperty("productUpdate")]
    public bool ProductUpdate { get; set; }

    [JsonProperty("productSaleService")]
    public bool ProductSaleService { get; set; }

    [JsonProperty("productSpecialService")]
    public bool ProductSpecialService { get; set; }

    [JsonProperty("productServiceAnalyze")]
    public bool ProductServiceAnalyze { get; set; }

    /// <summary>
    /// 是不是这个目录要显示
    /// </summary>
    [JsonProperty("projectManage")]
    public bool ProjectManage { get; set; }

    [JsonProperty("productSale")]
    public bool ProductSale { get; set; }

    [JsonProperty("productRent")]
    public bool ProductRent { get; set; }

    [JsonProperty("productEval")]
    public bool ProductEval { get; set; }

    [JsonProperty("projectAnalyze")]
    public bool ProjectAnalyze { get; set; }

    /// <summary>
    /// 是不是这个目录要显示
    /// </summary>
    [JsonProperty("basicSetting")]
    public bool BasicSetting { get; set; }

    [JsonProperty("product")]
    public bool Product { get; set; }

    [JsonProperty("college")]
    public bool College { get; set; }

    [JsonProperty("productUrl")]
    public bool ProductUrl { get; set; }

    /// <summary>
    /// 是不是这个目录要显示
    /// </summary>
    [JsonProperty("systemManage")]
    public bool SystemManage { get; set; }

    [JsonProperty("user")]
    public bool User { get; set; }

    [JsonProperty("group")]
    public bool Group { get; set; }

    [JsonProperty("passRes")]
    public bool PassRes { get; set; }

    [JsonProperty("actionLog")]
    public bool ActionLog { get; set; }
}