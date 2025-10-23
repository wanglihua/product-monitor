using Newtonsoft.Json;

namespace Web.Models.CollegeSelect;

public class CollegeSelectDataQueryParam
{
    [JsonProperty("search")]
    public string Search { get; set; } = "";
}