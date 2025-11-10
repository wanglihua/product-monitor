using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.GroupUser;

public class GroupUserListParam : ListParam
{
    [JsonProperty("groupId")]
    public long GroupId { get; set; }
}