using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ProductInstall;

public class ProductInstallListParam : ListParam
{
    [JsonProperty("collegeNameQuery")]
    public string CollegeNameQuery { get; set; } = "";
    
    [JsonProperty("productNameQuery")]
    public string ProductNameQuery { get; set; } = "";
}