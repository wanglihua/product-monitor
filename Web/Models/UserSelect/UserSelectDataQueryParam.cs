using Newtonsoft.Json;

namespace Web.Models.UserSelect;

public class UserSelectDataQueryParam
{
    [JsonProperty("search")]
    public string Search { get; set; } = "";
}