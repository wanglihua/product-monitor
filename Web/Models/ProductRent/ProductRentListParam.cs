
using Core.Web;
using Newtonsoft.Json;

namespace Web.Models.ProductRent;

public class ProductRentListParam : ListParam
{
    [JsonProperty("collegeNameQuery")]
    public string CollegeNameQuery { get; set; } = "";
    
    [JsonProperty("productNameQuery")]
    public string ProductNameQuery { get; set; } = "";
}