
using Newtonsoft.Json;

namespace Web.Models.ProductVersionSelect;

public class ProductVersionSelectDataQueryParam
{
    [JsonProperty("productId")]
    public long ProductId { get; set; }
}