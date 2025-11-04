using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.Product;

public class ProductDetailModel
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = "";

    [JsonProperty("englishName")]
    public string EnglishName { get; set; } = "";
}

public class ProductDetailModelValidator : AbstractValidator<ProductDetailModel>
{
    public ProductDetailModelValidator()
    {
        RuleFor(e => e.Name).NotNull().WithMessage("产品名称不能为空!");
        RuleFor(e => e.Name).NotEmpty().WithMessage("产品名称不能为空!");
        RuleFor(e => e.Name).MaximumLength(250).WithMessage("产品名称最大长度不能超过250!");

        RuleFor(e => e.EnglishName).NotNull().WithMessage("产品英文名称不能为空!");
        RuleFor(e => e.EnglishName).NotEmpty().WithMessage("产品英文名称不能为空!");
        RuleFor(e => e.EnglishName).MaximumLength(250).WithMessage("产品英文名称最大长度不能超过250!");
    }
}