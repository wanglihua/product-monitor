using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.AdminUser;

public class AdminUserListParam : ListParam
{
    [JsonProperty("loginNameQuery")]
    public string LoginNameQuery { get; set; } = "";

    [JsonProperty("userNameQuery")]
    public string UserNameQuery { get; set; } = "";
}