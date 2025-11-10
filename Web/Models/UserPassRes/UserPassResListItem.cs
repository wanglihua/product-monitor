using Newtonsoft.Json;

namespace Web.Models.UserPassRes;

public class UserPassResListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("userId")]
    public long UserId { get; set; }

    [JsonProperty("passResId")]
    public long PassResId { get; set; }

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";

    [JsonProperty("updateTime")]
    public string UpdateTime { get; set; } = "";
}