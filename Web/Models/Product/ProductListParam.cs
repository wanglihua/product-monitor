using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.Product;

public class ProductListParam : ListParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}