using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductSpecialService;

public class ProductSpecialServiceListParam : ListParam
{
    [JsonProperty("collegeNameQuery")]
    public string CollegeNameQuery { get; set; } = "";

    [JsonProperty("productNameQuery")]
    public string ProductNameQuery { get; set; } = "";
}

public class ProductSpecialServiceListParamValidator : AbstractValidator<ProductSpecialServiceListParam>
{
    public ProductSpecialServiceListParamValidator()
    {
    }
}