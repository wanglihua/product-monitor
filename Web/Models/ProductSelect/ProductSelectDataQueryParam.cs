using Newtonsoft.Json;

namespace Web.Models.ProductSelect;

public class ProductSelectDataQueryParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}