using Newtonsoft.Json;

namespace Web.Models.ActionLog;

public class ActionLogListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("loginName")]
    public string LoginName { get; set; } = string.Empty;

    [JsonProperty("userName")]
    public string UserName { get; set; } = string.Empty;

    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    [JsonProperty("actionDesc")]
    public string ActionDesc { get; set; } = string.Empty;

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = string.Empty;
}