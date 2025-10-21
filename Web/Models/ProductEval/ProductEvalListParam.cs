
using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ProductEval;

public class ProductEvalListParam : ListParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}