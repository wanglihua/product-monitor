
using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductUpdate;

public class ProductUpdateListParam : ListParam
{
    [JsonProperty("collegeNameQuery")]
    public string CollegeNameQuery { get; set; } = "";
    
    [JsonProperty("productNameQuery")]
    public string ProductNameQuery { get; set; } = "";
}

public class ProductUpdateListParamValidator : AbstractValidator<ProductUpdateListParam>
{
    public ProductUpdateListParamValidator()
    {
    }
}