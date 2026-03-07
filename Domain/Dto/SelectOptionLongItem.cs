using Newtonsoft.Json;

namespace Domain.Dto;

public class SelectOptionLongItem
{
    [JsonProperty("value")]
    public long Value { get; set; }

    [JsonProperty("label")]
    public string Label { get; set; } = "";
}