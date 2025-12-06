using Newtonsoft.Json;

namespace Web.Models.ClientKey;

public class ClientKeyListItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("collegeName")]
    public string CollegeName { get; set; } = "";

    [JsonProperty("productName")]
    public string ProductName { get; set; } = "";

    [JsonProperty("keyType")]
    public string KeyType { get; set; } = "";

    [JsonProperty("clientKey")]
    public string ClientKey { get; set; } = "";

    [JsonProperty("userName")]
    public string UserName { get; set; } = "";

    [JsonProperty("createTime")]
    public string CreateTime { get; set; } = "";

    [JsonProperty("updateTime")]
    public string UpdateTime { get; set; } = "";
}