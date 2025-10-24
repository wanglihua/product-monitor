using Newtonsoft.Json;

namespace Web.Models.ProductRent;

public class ProductRentListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("collegeName")]
    public string CollegeName { get; set; } = "";

    [JsonProperty("productName")]
    public string ProductName { get; set; } = "";

    [JsonProperty("rentTimes")]
    public int RentTimes { get; set; }

    [JsonProperty("serviceTimes")]
    public int ServiceTimes { get; set; }

    [JsonProperty("totalDays")]
    public int TotalDays { get; set; }

    [JsonProperty("remainDays")]
    public int RemainDays { get; set; }

    [JsonProperty("beginDate")]
    public string BeginDate { get; set; } = "";

    [JsonProperty("endDate")]
    public string EndDate { get; set; } = "";

    [JsonProperty("description")]
    public string Description { get; set; } = "";

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";

    [JsonProperty("updateTime")]
    public string UpdateTime { get; set; } = "";
}