using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.Product;

public class ProductListParam : ListParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";

    [JsonProperty("englishNameQuery")]
    public string EnglishNameQuery { get; set; } = "";
}

public class ProductListParamValidator : AbstractValidator<ProductListParam>
{
    public ProductListParamValidator()
    {
    }
}