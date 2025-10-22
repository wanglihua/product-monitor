
using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.College;

public class CollegeListParam : ListParam
{
    [JsonProperty("codeQuery")]
    public string CodeQuery { get; set; } = "";
    
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}