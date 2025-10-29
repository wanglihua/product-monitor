using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductVersion;

public class ProductVersionDetailParam
{
    [JsonProperty("productVersionId")]
    public long ProductVersionId { get; set; }

    [JsonProperty("productId")]
    public long ProductId { get; set; }
}

public class ProductVersionDetailParamValidator : AbstractValidator<ProductVersionDetailParam>
{
    public ProductVersionDetailParamValidator()
    {
        RuleFor(e => e.ProductId).Must(p => p > 0).WithMessage("产品不能为空!");
    }
}