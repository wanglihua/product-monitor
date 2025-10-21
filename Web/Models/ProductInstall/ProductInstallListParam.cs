
using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ProductInstall;

public class ProductInstallListParam : ListParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}