using Newtonsoft.Json;

namespace Web.Models.ProductSelect;

public class ProductSelectBySaleQueryParam
{
    [JsonProperty("collegeId")]
    public long CollegeId { get; set; }
}