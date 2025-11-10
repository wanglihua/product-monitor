using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductInstall;

public class ProductInstallListParam : ListParam
{
    [JsonProperty("collegeNameQuery")]
    public string CollegeNameQuery { get; set; } = "";
    
    [JsonProperty("productNameQuery")]
    public string ProductNameQuery { get; set; } = "";
}

public class ProductInstallListParamValidator : AbstractValidator<ProductInstallListParam>
{
    public ProductInstallListParamValidator()
    {
    }
}