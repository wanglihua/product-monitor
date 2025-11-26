using Core.Web;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Models.Product;

public class ProductListParam : ListParam
{
    [BindProperty(Name = "nameQuery")]
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";

    [BindProperty(Name = "englishNameQuery")]
    [JsonProperty("englishNameQuery")]
    public string EnglishNameQuery { get; set; } = "";
}

public class ProductListParamValidator : AbstractValidator<ProductListParam>
{
    public ProductListParamValidator()
    {
    }
}