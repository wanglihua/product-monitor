
using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ProductEvalEntry;

public class ProductEvalEntryListParam : ListParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}