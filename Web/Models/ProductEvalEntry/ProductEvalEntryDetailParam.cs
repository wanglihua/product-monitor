using Newtonsoft.Json;

namespace Web.Models.ProductEvalEntry;

public class ProductEvalEntryDetailParam
{
    [JsonProperty("productEvalEntryId")]
    public long ProductEvalEntryId { get; set; }

    [JsonProperty("productEvalId")]
    public long ProductEvalId { get; set; }
}