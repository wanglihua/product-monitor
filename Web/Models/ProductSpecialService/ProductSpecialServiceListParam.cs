using Core.Web;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Models.ProductSpecialService;

public class ProductSpecialServiceListParam : ListParam
{
    [BindProperty(Name = "collegeNameQuery")]
    [JsonProperty("collegeNameQuery")]
    public string CollegeNameQuery { get; set; } = "";

    [BindProperty(Name = "productNameQuery")]
    [JsonProperty("productNameQuery")]
    public string ProductNameQuery { get; set; } = "";

    [BindProperty(Name = "createDateBeginQuery")]
    [JsonProperty("createDateBeginQuery")]
    public string CreateDateBeginQuery { get; set; } = "";

    [BindProperty(Name = "createDateEndQuery")]
    [JsonProperty("createDateEndQuery")]
    public string CreateDateEndQuery { get; set; } = "";
}

public class ProductSpecialServiceListParamValidator : AbstractValidator<ProductSpecialServiceListParam>
{
    public ProductSpecialServiceListParamValidator()
    {
    }
}