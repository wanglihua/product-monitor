using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductVersion;

public class ProductVersionDetailModel
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("productId")]
    public long ProductId { get; set; }

    [JsonProperty("version")]
    public string Version { get; set; } = "";

    [JsonProperty("versionUpdateBase")]
    public string VersionUpdateBase { get; set; } = "";

    [JsonProperty("versionUpdateDesc")]
    public string VersionUpdateDesc { get; set; } = "";

    [JsonProperty("versionUpdateTime")]
    public string VersionUpdateTime { get; set; } = "";
}

public class ProductVersionDetailModelValidator : AbstractValidator<ProductVersionDetailModel>
{
    public ProductVersionDetailModelValidator()
    {
        RuleFor(e => e.ProductId).Must(p => p > 0).WithMessage("产品目录不能为空!");

        RuleFor(e => e.Version).NotNull().WithMessage("版本号不能为空!");
        RuleFor(e => e.Version).NotEmpty().WithMessage("版本号不能为空!");
        RuleFor(e => e.Version).MaximumLength(32).WithMessage("版本号最大长度不能超过32!");

        RuleFor(e => e.VersionUpdateBase).NotNull().WithMessage("更新基础不能为空!");
        RuleFor(e => e.VersionUpdateBase).NotEmpty().WithMessage("更新基础不能为空!");
        RuleFor(e => e.VersionUpdateBase).MaximumLength(32).WithMessage("更新基础最大长度不能超过32!");

        RuleFor(e => e.VersionUpdateDesc).MaximumLength(512).WithMessage("更新内容最大长度不能超过512!");

        RuleFor(e => e.VersionUpdateTime).NotNull().WithMessage("版本更新时间不能为空!");
        RuleFor(e => e.VersionUpdateTime).NotEmpty().WithMessage("版本更新时间不能为空!");
        RuleFor(e => e.VersionUpdateTime)
            .Must(p => DateTime.TryParse(p, out var versionUpdateTime))
            .WithMessage("版本更新时间格式不正确!");
    }
}