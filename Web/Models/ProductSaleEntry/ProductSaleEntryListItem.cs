using Newtonsoft.Json;

namespace Web.Models.ProductSaleEntry;

public class ProductSaleEntryListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("productSaleId")]
    public long ProductSaleId { get; set; }

    [JsonProperty("version")]
    public string Version { get; set; } = "";

    [JsonProperty("saleDate")]
    public string SaleDate { get; set; } = "";

    [JsonProperty("serviceExpireDate")]
    public string ServiceExpireDate { get; set; } = "";

    [JsonProperty("description")]
    public string Description { get; set; } = "";

    [JsonProperty("userName")]
    public string UserName { get; set; } = "";

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";

    [JsonProperty("updateTime")]
    public string UpdateTime { get; set; } = "";
}