using Newtonsoft.Json;

namespace Web.Models.CollegeChangeLog;

public class CollegeChangeLogListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("formerName")]
    public string FormerName { get; set; } = "";

    [JsonProperty("changeDesc")]
    public string ChangeDesc { get; set; } = "";

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";

    [JsonProperty("updateTime")]
    public string UpdateTime { get; set; } = "";
}