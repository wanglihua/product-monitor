using Newtonsoft.Json;

namespace Web.Models.ProductUrlMonitorEntry;

public class ProductUrlMonitorEntryListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("productUrlMonitorId")]
    public long ProductUrlMonitorId { get; set; }

    [JsonProperty("loginUser")]
    public string LoginUser { get; set; } = "";

    [JsonProperty("loginPassword")]
    public string LoginPassword { get; set; } = "";

    [JsonProperty("ipAddress")]
    public string IpAddress { get; set; } = "";

    [JsonProperty("provinceType")]
    public string ProvinceType { get; set; } = "";

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";

    [JsonProperty("updateTime")]
    public string UpdateTime { get; set; } = "";
}