
using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ProductUpdate;

public class ProductUpdateListParam : ListParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}