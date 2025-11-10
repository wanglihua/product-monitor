using Newtonsoft.Json;

namespace Web.Models.User;

public class UserListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("loginName")]
    public string LoginName { get; set; } = "";

    [JsonProperty("userName")]
    public string UserName { get; set; } = "";

    [JsonProperty("lastLoginTime")]
    public string LastLoginTime { get; set; } = "";

    [JsonProperty("status")]
    public string Status { get; set; } = "";

    [JsonProperty("enabled")]
    public bool Enabled { get; set; } = true;

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";
}