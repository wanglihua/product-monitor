using Newtonsoft.Json;

namespace Web.Models.ProductInstallMonitorEntry;

public class ProductInstallMonitorEntryListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("loginUser")]
    public string LoginUser { get; set; } = "";

    [JsonProperty("accountType")]
    public string AccountType { get; set; } = "";

    [JsonProperty("ipAddress")]
    public string IpAddress { get; set; } = "";

    [JsonProperty("provinceType")]
    public string ProvinceType { get; set; } = "";

    [JsonProperty("useTime")]
    public string UseTime { get; set; } = "";

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";

    [JsonProperty("updateTime")]
    public string UpdateTime { get; set; } = "";
}