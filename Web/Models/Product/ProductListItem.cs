using Newtonsoft.Json;

namespace Web.Models.Product;

public class ProductListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = "";

    [JsonProperty("version")]
    public string Version { get; set; } = "";

    [JsonProperty("versionUpdateBase")]
    public string VersionUpdateBase { get; set; } = "";

    [JsonProperty("versionUpdateDesc")]
    public string VersionUpdateDesc { get; set; } = "";

    [JsonProperty("versionUpdateTime")]
    public string VersionUpdateTime { get; set; } = "";

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";

    [JsonProperty("updateTime")]
    public string UpdateTime { get; set; } = "";
}