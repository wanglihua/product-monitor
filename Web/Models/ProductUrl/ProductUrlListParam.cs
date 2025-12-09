using Core.CodeType;
using Core.Web;
using Domain;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Models.ProductUrl;

public class ProductUrlListParam : ListParam
{
    [BindProperty(Name = "typeQuery")]
    [JsonProperty("typeQuery")]
    [JsonConverter(typeof(ProductUrlTypeJsonConverter))]
    [ModelBinder<CodeTypeModelBinder<ProductUrlType>>]
    public ProductUrlType TypeQuery { get; set; } = ProductUrlType.None;

    [BindProperty(Name = "nameQuery")]
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}

public class ProductUrlListParamValidator : AbstractValidator<ProductUrlListParam>
{
    public ProductUrlListParamValidator()
    {
    }
}