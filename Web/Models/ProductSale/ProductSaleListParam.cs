
using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ProductSale;

public class ProductSaleListParam : ListParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}