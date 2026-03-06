using Newtonsoft.Json;

namespace Web.Models.ProductSelect;

public class ProductSelectByEvalQueryParam
{
    [JsonProperty("collegeId")]
    public long CollegeId { get; set; }
}