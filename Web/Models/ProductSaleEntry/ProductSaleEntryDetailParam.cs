using Newtonsoft.Json;

namespace Web.Models.ProductSaleEntry;

public class ProductSaleEntryDetailParam
{
    [JsonProperty("productSaleEntryId")]
    public long ProductSaleEntryId { get; set; }

    [JsonProperty("productSaleId")]
    public long ProductSaleId { get; set; }
}