using System.Globalization;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.Product;

public class ProductDetailModel
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = "";

    [JsonProperty("version")]
    public string Version { get; set; } = "";

    [JsonProperty("versionUpdateBase")]
    public string VersionUpdateBase { get; set; } = "";

    [JsonProperty("versionUpdateDesc")]
    public string VersionUpdateDesc { get; set; } = "";

    [JsonProperty("versionUpdateTime")]
    public string VersionUpdateTime { get; set; } = "";
}

public class ProductDetailModelValidator : AbstractValidator<ProductDetailModel>
{
    public ProductDetailModelValidator()
    {
        RuleFor(e => e.Name).NotNull().WithMessage("产品名称不能为空!");
        RuleFor(e => e.Name).NotEmpty().WithMessage("产品名称不能为空!");
        RuleFor(e => e.Name).MaximumLength(250).WithMessage("产品名称最大长度不能超过250!");

        RuleFor(e => e.Version).NotNull().WithMessage("版本号不能为空!");
        RuleFor(e => e.Version).NotEmpty().WithMessage("版本号不能为空!");
        RuleFor(e => e.Version).MaximumLength(30).WithMessage("版本号最大长度不能超过30!");

        RuleFor(e => e.VersionUpdateBase).MaximumLength(30).WithMessage("更新基础最大长度不能超过30!");

        RuleFor(e => e.VersionUpdateDesc).MaximumLength(500).WithMessage("更新内容最大长度不能超过500!");

        RuleFor(e => e.VersionUpdateTime)
            .Must(p => DateTime.TryParseExact(p, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var versionUpdateTime))
            .WithMessage("版本更新时间格式不正确!");
    }
}