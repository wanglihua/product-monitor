using Newtonsoft.Json;

namespace Core.Web;

public class JsonResultData
{
    [JsonProperty("success")]
    public bool Success { get; set; } = true;

    [JsonProperty("message")]
    public string Message { get; set; } = "success";

    [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
    public object? Data { get; set; } = null;
}

public class JsonResultData<T> where T : class
{
    [JsonProperty("success")]
    public bool Success { get; set; } = true;

    [JsonProperty("message")]
    public string Message { get; set; } = "success";

    [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
    public T? Data { get; set; } = null;
}