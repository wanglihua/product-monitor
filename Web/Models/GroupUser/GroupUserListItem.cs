using Newtonsoft.Json;

namespace Web.Models.GroupUser;

public class GroupUserListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("groupName")]
    public string GroupName { get; set; } = "";

    [JsonProperty("userName")]
    public string UserName { get; set; } = "";

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";
}