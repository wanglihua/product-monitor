
using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ProductRentEntry;

public class ProductRentEntryListParam : ListParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}