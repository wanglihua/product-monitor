using Newtonsoft.Json;

namespace Web.Models.PassRes;

public class PassResListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("cateName")]
    public string CateName { get; set; } = "";

    [JsonProperty("name")]
    public string Name { get; set; } = "";

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";

    [JsonProperty("updateTime")]
    public string UpdateTime { get; set; } = "";
}