using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ProductUrlMonitorEntry;

public class ProductUrlMonitorEntryListParam : ListParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}