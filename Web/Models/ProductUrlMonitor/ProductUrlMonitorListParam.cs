using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ProductUrlMonitor;

public class ProductUrlMonitorListParam : ListParam
{
    [JsonProperty("collegeNameQuery")]
    public string CollegeNameQuery { get; set; } = "";
    
    [JsonProperty("productNameQuery")]
    public string ProductNameQuery { get; set; } = "";
}