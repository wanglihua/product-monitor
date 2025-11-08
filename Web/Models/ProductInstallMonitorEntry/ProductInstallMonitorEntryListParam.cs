using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ProductInstallMonitorEntry;

public class ProductInstallMonitorEntryListParam : ListParam
{
    [JsonProperty("productInstallMonitorId")]
    public long ProductInstallMonitorId { get; set; }
}