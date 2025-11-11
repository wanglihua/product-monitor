using Newtonsoft.Json;

namespace Web.Models.GroupPassRes;

public class PassResGroupListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("groupId")]
    public long GroupId { get; set; }

    [JsonProperty("groupName")]
    public string GroupName { get; set; } = "";

    [JsonProperty("grant")]
    public bool Grant { get; set; }

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";
}