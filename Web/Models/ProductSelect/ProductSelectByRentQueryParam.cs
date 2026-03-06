using Newtonsoft.Json;

namespace Web.Models.ProductSelect;

public class ProductSelectByRentQueryParam
{
    [JsonProperty("collegeId")]
    public long CollegeId { get; set; }
}