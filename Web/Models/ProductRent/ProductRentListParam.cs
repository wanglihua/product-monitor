
using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ProductRent;

public class ProductRentListParam : ListParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}