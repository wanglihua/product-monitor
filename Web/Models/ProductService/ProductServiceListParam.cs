
using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ProductService;

public class ProductServiceListParam : ListParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}