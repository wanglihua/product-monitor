using Newtonsoft.Json;

namespace Web.Models.GroupPassRes;

public class GroupPassResListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("groupId")]
    public long GroupId { get; set; }

    [JsonProperty("passResId")]
    public long PassResId { get; set; }

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";

    [JsonProperty("updateTime")]
    public string UpdateTime { get; set; } = "";
}