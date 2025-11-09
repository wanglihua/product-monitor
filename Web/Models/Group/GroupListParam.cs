using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.Group;

public class GroupListParam : ListParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}