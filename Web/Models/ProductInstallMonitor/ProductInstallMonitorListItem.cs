using Newtonsoft.Json;

namespace Web.Models.ProductInstallMonitor;

public class ProductInstallMonitorListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("collegeName")]
    public string CollegeName { get; set; } = "";

    [JsonProperty("productName")]
    public string ProductName { get; set; } = "";

    [JsonProperty("province")]
    public string Province { get; set; } = "";

    [JsonProperty("cpuSerialNo")]
    public string CpuSerialNo { get; set; } = "";

    [JsonProperty("netMacAddress")]
    public string NetMacAddress { get; set; } = "";

    [JsonProperty("taskCount")]
    public int TaskCount { get; set; }

    [JsonProperty("teacherCount")]
    public int TeacherCount { get; set; }

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";

    [JsonProperty("updateTime")]
    public string UpdateTime { get; set; } = "";
}