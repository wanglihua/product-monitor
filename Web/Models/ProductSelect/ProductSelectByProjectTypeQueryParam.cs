using Newtonsoft.Json;

namespace Web.Models.ProductSelect;

public class ProductSelectByProjectTypeQueryParam
{
    [JsonProperty("projectTypeId")]
    public int ProjectTypeId { get; set; }
    
    [JsonProperty("collegeId")]
    public long CollegeId { get; set; }
}