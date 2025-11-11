using Newtonsoft.Json;

namespace Web.Models.GroupPassRes;

public class GroupPassResListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("passResId")]
    public long PassResId { get; set; }

    [JsonProperty("passResName")]
    public string PassResName { get; set; } = "";

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";
}