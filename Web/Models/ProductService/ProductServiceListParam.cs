
using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductService;

public class ProductServiceListParam : ListParam
{
    [JsonProperty("collegeNameQuery")]
    public string CollegeNameQuery { get; set; } = "";
    
    [JsonProperty("productNameQuery")]
    public string ProductNameQuery { get; set; } = "";
}

public class ProductServiceListParamValidator : AbstractValidator<ProductServiceListParam>
{
    public ProductServiceListParamValidator()
    {
    }
}