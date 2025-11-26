using Core.Web;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Models.ProductUpdate;

public class ProductUpdateListParam : ListParam
{
    [BindProperty(Name = "collegeNameQuery")]
    [JsonProperty("collegeNameQuery")]
    public string CollegeNameQuery { get; set; } = "";

    [BindProperty(Name = "productNameQuery")]
    [JsonProperty("productNameQuery")]
    public string ProductNameQuery { get; set; } = "";
}

public class ProductUpdateListParamValidator : AbstractValidator<ProductUpdateListParam>
{
    public ProductUpdateListParamValidator()
    {
    }
}