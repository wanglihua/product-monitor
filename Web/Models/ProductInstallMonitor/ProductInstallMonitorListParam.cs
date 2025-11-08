using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ProductInstallMonitor;

public class ProductInstallMonitorListParam : ListParam
{
    [JsonProperty("collegeNameQuery")]
    public string CollegeNameQuery { get; set; } = "";
    
    [JsonProperty("productNameQuery")]
    public string ProductNameQuery { get; set; } = "";
}