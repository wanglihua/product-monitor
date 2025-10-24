using Newtonsoft.Json;

namespace Web.Models.ProductSale;

public class ProductSaleListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("collegeName")]
    public string CollegeName { get; set; } = "";

    [JsonProperty("productName")]
    public string ProductName { get; set; } = "";

    [JsonProperty("saleTimes")]
    public int SaleTimes { get; set; }

    [JsonProperty("installTimes")]
    public int InstallTimes { get; set; }

    [JsonProperty("updateTimes")]
    public int UpdateTimes { get; set; }

    [JsonProperty("serviceTimes")]
    public int ServiceTimes { get; set; }

    [JsonProperty("version")]
    public string Version { get; set; } = "";

    [JsonProperty("saleDate")]
    public string SaleDate { get; set; } = "";

    [JsonProperty("serviceExpireDate")]
    public string ServiceExpireDate { get; set; } = "";

    [JsonProperty("description")]
    public string Description { get; set; } = "";

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";

    [JsonProperty("updateTime")]
    public string UpdateTime { get; set; } = "";
}