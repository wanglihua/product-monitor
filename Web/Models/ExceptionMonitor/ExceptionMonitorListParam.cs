using Core.CodeType;
using Core.Web;
using Domain;
using Newtonsoft.Json;

namespace Web.Models.ExceptionMonitor;

public class ExceptionMonitorListParam : ListParam
{
    [JsonProperty("productNameQuery")]
    public string ProductNameQuery { get; set; } = "";

    [JsonProperty("provinceTypeQuery")]
    [JsonConverter(typeof(ProvinceTypeJsonConverter))]
    public ProvinceType ProvinceTypeQuery { get; set; } = ProvinceType.None;
}