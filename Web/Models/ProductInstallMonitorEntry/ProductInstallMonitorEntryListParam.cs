using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ProductInstallMonitorEntry;

public class ProductInstallMonitorEntryListParam : ListParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}