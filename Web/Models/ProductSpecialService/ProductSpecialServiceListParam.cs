using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ProductSpecialService;

public class ProductSpecialServiceListParam : ListParam
{
    [JsonProperty("collegeNameQuery")]
    public string CollegeNameQuery { get; set; } = "";

    [JsonProperty("productNameQuery")]
    public string ProductNameQuery { get; set; } = "";
}