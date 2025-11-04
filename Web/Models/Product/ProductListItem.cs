using Newtonsoft.Json;

namespace Web.Models.Product;

public class ProductListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = "";

    [JsonProperty("englishName")]
    public string EnglishName { get; set; } = "";

    [JsonProperty("versionCount")]
    public int VersionCount { get; set; }

    [JsonProperty("latestVersion")]
    public string LatestVersion { get; set; } = "";

    [JsonProperty("versionUpdateTime")]
    public string VersionUpdateTime { get; set; } = "";

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";

    [JsonProperty("updateTime")]
    public string UpdateTime { get; set; } = "";
}