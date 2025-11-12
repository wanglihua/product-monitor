using Newtonsoft.Json;

namespace Web.Models.AuthResult;

public class AuthResultListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("passResCateName")]
    public string PassResCateName { get; set; } = "";

    [JsonProperty("passResName")]
    public string PassResName { get; set; } = "";

    [JsonProperty("loginName")]
    public string LoginName { get; set; } = "";

    [JsonProperty("userName")]
    public string UserName { get; set; } = "";

    [JsonProperty("groupName")]
    public string GroupName { get; set; } = "";

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";
}