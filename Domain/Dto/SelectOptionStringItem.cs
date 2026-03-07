using Newtonsoft.Json;

namespace Domain.Dto;

public class SelectOptionStringItem
{
    [JsonProperty("value")]
    public string Value { get; set; } = "";

    [JsonProperty("label")]
    public string Label { get; set; } = "";
}