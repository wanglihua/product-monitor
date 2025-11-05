using Newtonsoft.Json;

namespace Web.Models.ProductEvalEntry;

public class ProductEvalEntryListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("productEvalId")]
    public long ProductEvalId { get; set; }

    [JsonProperty("totalDays")]
    public int TotalDays { get; set; }

    [JsonProperty("beginDate")]
    public string BeginDate { get; set; } = "";

    [JsonProperty("endDate")]
    public string EndDate { get; set; } = "";

    [JsonProperty("description")]
    public string Description { get; set; } = "";

    [JsonProperty("userName")]
    public string UserName { get; set; } = "";

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";

    [JsonProperty("updateTime")]
    public string UpdateTime { get; set; } = "";
}