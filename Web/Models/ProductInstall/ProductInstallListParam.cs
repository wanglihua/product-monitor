using Core.Web;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Models.ProductInstall;

public class ProductInstallListParam : ListParam
{
    [BindProperty(Name = "collegeNameQuery")]
    [JsonProperty("collegeNameQuery")]
    public string CollegeNameQuery { get; set; } = "";
    
    [BindProperty(Name = "productNameQuery")]
    [JsonProperty("productNameQuery")]
    public string ProductNameQuery { get; set; } = "";
}

public class ProductInstallListParamValidator : AbstractValidator<ProductInstallListParam>
{
    public ProductInstallListParamValidator()
    {
    }
}