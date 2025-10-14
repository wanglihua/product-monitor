using Newtonsoft.Json;

namespace Web.Models.ActionLog;

public class ActionLogListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("loginName")]
    public string LoginName { get; set; } = "";

    [JsonProperty("userName")]
    public string UserName { get; set; } = "";

    [JsonProperty("type")]
    public string Type { get; set; } = "";

    [JsonProperty("actionDesc")]
    public string ActionDesc { get; set; } = "";

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";
}