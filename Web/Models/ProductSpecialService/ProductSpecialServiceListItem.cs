using Newtonsoft.Json;

namespace Web.Models.ProductSpecialService;

public class ProductSpecialServiceListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("collegeName")]
    public string CollegeName { get; set; } = "";

    [JsonProperty("productName")]
    public string ProductName { get; set; } = "";

    [JsonProperty("serviceDate")]
    public string ServiceDate { get; set; } = "";

    [JsonProperty("serviceCount")]
    public int ServiceCount { get; set; }

    [JsonProperty("serviceUrl")]
    public string ServiceUrl { get; set; } = "";

    [JsonProperty("loginUser")]
    public string LoginUser { get; set; } = "";

    [JsonProperty("loginPassword")]
    public string LoginPassword { get; set; } = "";

    [JsonProperty("contactTeacher")]
    public string ContactTeacher { get; set; } = "";

    [JsonProperty("contactMobile")]
    public string ContactMobile { get; set; } = "";

    [JsonProperty("description")]
    public string Description { get; set; } = "";

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";

    [JsonProperty("updateTime")]
    public string UpdateTime { get; set; } = "";

    [JsonProperty("userName")]
    public string UserName { get; set; } = "";
}