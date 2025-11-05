using Newtonsoft.Json;

namespace Web.Models.ProductInstallMonitor;

public class ProductInstallMonitorListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("collegeId")]
    public string CollegeId { get; set; } = ""; 

    [JsonProperty("productId")]
    public string ProductId { get; set; } = ""; 

    [JsonProperty("provinceType")]
    public string ProvinceType { get; set; } = ""; 

    [JsonProperty("cpuSerialNo")]
    public string CpuSerialNo { get; set; } = ""; 

    [JsonProperty("netMacAddress")]
    public string NetMacAddress { get; set; } = ""; 

    [JsonProperty("taskCount")]
    public string TaskCount { get; set; } = ""; 

    [JsonProperty("teacherCount")]
    public string TeacherCount { get; set; } = ""; 

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";

    [JsonProperty("updateTime")]
    public string UpdateTime { get; set; } = "";
}