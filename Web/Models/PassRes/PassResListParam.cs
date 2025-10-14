using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.PassRes;

public class PassResListParam : ListParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}