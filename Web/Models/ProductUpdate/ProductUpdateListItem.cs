using Newtonsoft.Json;

namespace Web.Models.ProductUpdate;

public class ProductUpdateListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("collegeName")]
    public string CollegeName { get; set; } = "";

    [JsonProperty("productName")]
    public string ProductName { get; set; } = "";

    [JsonProperty("versionSale")]
    public string VersionSale { get; set; } = "";

    [JsonProperty("versionUpdate")]
    public string VersionUpdate { get; set; } = "";

    [JsonProperty("updateCount")]
    public int UpdateCount { get; set; }

    [JsonProperty("updateUrl")]
    public string UpdateUrl { get; set; } = "";

    [JsonProperty("loginUser")]
    public string LoginUser { get; set; } = "";

    [JsonProperty("loginPassword")]
    public string LoginPassword { get; set; } = "";

    [JsonProperty("updateDate")]
    public string UpdateDate { get; set; } = "";

    [JsonProperty("salesManager")]
    public string SalesManager { get; set; } = "";

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