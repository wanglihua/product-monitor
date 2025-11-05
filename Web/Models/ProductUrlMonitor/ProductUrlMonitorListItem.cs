using Newtonsoft.Json;

namespace Web.Models.ProductUrlMonitor;

public class ProductUrlMonitorListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("collegeId")]
    public long CollegeId { get; set; }

    [JsonProperty("productId")]
    public long ProductId { get; set; }

    [JsonProperty("productUrlType")]
    public string ProductUrlType { get; set; } = "";

    [JsonProperty("taskCount")]
    public int TaskCount { get; set; }

    [JsonProperty("teacherCount")]
    public int TeacherCount { get; set; }

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";

    [JsonProperty("updateTime")]
    public string UpdateTime { get; set; } = "";
}