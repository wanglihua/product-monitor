using Newtonsoft.Json;

namespace Web.Models.ProductUrl;

public class ProductUrlListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = ""; 

    [JsonProperty("url")]
    public string Url { get; set; } = ""; 

    [JsonProperty("typeName")]
    public string TypeName { get; set; } = ""; 

    [JsonProperty("loginUser")]
    public string LoginUser { get; set; } = ""; 

    [JsonProperty("loginPassword")]
    public string LoginPassword { get; set; } = ""; 

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";

    [JsonProperty("updateTime")]
    public string UpdateTime { get; set; } = "";
}