
using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ProductUrl;

public class ProductUrlListParam : ListParam
{
    [JsonProperty("typeQuery")]
    public int TypeQuery { get; set; }
    
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}