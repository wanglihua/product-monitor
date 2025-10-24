
using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ProductSale;

public class ProductSaleListParam : ListParam
{
    [JsonProperty("collegeNameQuery")]
    public string CollegeNameQuery { get; set; } = "";
    
    [JsonProperty("productNameQuery")]
    public string ProductNameQuery { get; set; } = "";
}