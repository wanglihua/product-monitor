using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ExceptionMonitor;

public class ExceptionMonitorListParam : ListParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}