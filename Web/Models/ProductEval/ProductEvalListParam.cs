
using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ProductEval;

public class ProductEvalListParam : ListParam
{
    [JsonProperty("collegeNameQuery")]
    public string CollegeNameQuery { get; set; } = "";
    
    [JsonProperty("productNameQuery")]
    public string ProductNameQuery { get; set; } = "";
}