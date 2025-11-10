
using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductEval;

public class ProductEvalListParam : ListParam
{
    [JsonProperty("collegeNameQuery")]
    public string CollegeNameQuery { get; set; } = "";
    
    [JsonProperty("productNameQuery")]
    public string ProductNameQuery { get; set; } = "";
}

public class ProductEvalListParamValidator : AbstractValidator<ProductEvalListParam>
{
    public ProductEvalListParamValidator()
    {
    }
}