using Core.Web;
using Domain;
using Newtonsoft.Json;

namespace Web.Models.ProductUrl;

public class ProductUrlListParam : ListParam
{
    [JsonProperty("typeQuery")]
    [JsonConverter(typeof(ProductUrlTypeJsonConverter))]
    public ProductUrlType TypeQuery { get; set; } = ProductUrlType.None;

    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}