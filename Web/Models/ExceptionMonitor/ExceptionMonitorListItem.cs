using Newtonsoft.Json;

namespace Web.Models.ExceptionMonitor;

public class ExceptionMonitorListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("productName")]
    public string ProductName { get; set; } = "";

    [JsonProperty("ipAddress")]
    public string IpAddress { get; set; } = "";

    [JsonProperty("provinceType")]
    public string ProvinceType { get; set; } = "";

    [JsonProperty("port")]
    public string Port { get; set; } = "";

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";

    [JsonProperty("updateTime")]
    public string UpdateTime { get; set; } = "";
}