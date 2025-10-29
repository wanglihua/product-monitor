using Newtonsoft.Json;

namespace Web.Models.ProductVersion;

public class ProductVersionListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("productName")]
    public string ProductName { get; set; } = ""; 

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