using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ExceptionMonitor;

public class ExceptionMonitorListParam : ListParam
{
    [JsonProperty("productNameQuery")]
    public string ProductNameQuery { get; set; } = "";

    [JsonProperty("provinceTypeQuery")]
    public int ProvinceTypeQuery { get; set; }
}