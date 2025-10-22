
using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ProductSpecialService;

public class ProductSpecialServiceListParam : ListParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}