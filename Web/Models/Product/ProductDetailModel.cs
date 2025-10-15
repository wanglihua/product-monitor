using Newtonsoft.Json;

namespace Web.Models.Product;

public class ProductDetailModel
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
}