using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ActionLog;

public class ActionLogListParam : ListParam
{
    [JsonProperty("loginNameQuery")]
    public string LoginNameQuery { get; set; } = string.Empty;
}
