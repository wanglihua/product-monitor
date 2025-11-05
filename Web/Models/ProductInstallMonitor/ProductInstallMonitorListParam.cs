using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ProductInstallMonitor;

public class ProductInstallMonitorListParam : ListParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}