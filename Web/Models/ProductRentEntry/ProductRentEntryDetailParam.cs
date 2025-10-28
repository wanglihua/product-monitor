using Newtonsoft.Json;

namespace Web.Models.ProductRentEntry;

public class ProductRentEntryDetailParam
{
    [JsonProperty("productRentEntryId")]
    public long ProductRentEntryId { get; set; }

    [JsonProperty("productRentId")]
    public long ProductRentId { get; set; }
}