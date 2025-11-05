using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ProductUrlMonitor;

public class ProductUrlMonitorListParam : ListParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}