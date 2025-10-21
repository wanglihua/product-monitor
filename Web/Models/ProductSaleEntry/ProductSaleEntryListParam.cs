
using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ProductSaleEntry;

public class ProductSaleEntryListParam : ListParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}