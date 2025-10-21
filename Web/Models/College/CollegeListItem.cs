using Newtonsoft.Json;

namespace Web.Models.College;

public class CollegeListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("code")]
    public string Code { get; set; } = ""; 

    [JsonProperty("name")]
    public string Name { get; set; } = ""; 

    [JsonProperty("province")]
    public string Province { get; set; } = ""; 

    [JsonProperty("level")]
    public string Level { get; set; } = ""; 

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";

    [JsonProperty("updateTime")]
    public string UpdateTime { get; set; } = "";
}