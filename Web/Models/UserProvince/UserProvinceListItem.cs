using Newtonsoft.Json;

namespace Web.Models.UserProvince;

public class UserProvinceListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("loginName")]
    public string LoginName { get; set; } = "";

    [JsonProperty("userName")]
    public string UserName { get; set; } = "";

    [JsonProperty("provinceType")]
    public string ProvinceType { get; set; } = "";

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";

    [JsonProperty("updateTime")]
    public string UpdateTime { get; set; } = "";
}