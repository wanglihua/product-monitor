using Newtonsoft.Json;

namespace Core.Web;

public class SelectOption
{
    [JsonProperty("value")]
    public long Value { get; set; }

    [JsonProperty("label")]
    public string Label { get; set; } = "";
}