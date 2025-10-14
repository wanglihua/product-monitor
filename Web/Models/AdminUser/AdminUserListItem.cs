using Newtonsoft.Json;

namespace Web.Models.AdminUser;

public class AdminUserListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("loginName")]
    public string LoginName { get; set; } = "";

    [JsonProperty("userName")]
    public string UserName { get; set; } = "";

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";
}