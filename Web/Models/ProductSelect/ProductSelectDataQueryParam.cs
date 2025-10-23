using Newtonsoft.Json;

namespace Web.Models.ProductSelect;

public class ProductSelectDataQueryParam
{
    [JsonProperty("search")]
    public string Search { get; set; } = "";
}