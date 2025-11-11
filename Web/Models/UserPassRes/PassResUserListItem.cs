using Newtonsoft.Json;

namespace Web.Models.UserPassRes;

public class PassResUserListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("userId")]
    public long UserId { get; set; }

    [JsonProperty("loginName")]
    public string LoginName { get; set; } = "";

    [JsonProperty("userName")]
    public string UserName { get; set; } = "";

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";
}