using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductVersion;

public class ProductVersionListParam : ListParam
{
    [JsonProperty("productId")]
    public long ProductId { get; set; }
}

public class ProductVersionListParamValidator : AbstractValidator<ProductVersionListParam>
{
    public ProductVersionListParamValidator()
    {
        RuleFor(e => e.ProductId).Must(p => p > 0).WithMessage("产品不能为空!");
    }
}