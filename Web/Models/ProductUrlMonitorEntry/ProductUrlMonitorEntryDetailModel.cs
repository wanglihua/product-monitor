using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductUrlMonitorEntry;

public class ProductUrlMonitorEntryDetailModel
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("productUrlMonitorId")]
    public long ProductUrlMonitorId { get; set; }

    [JsonProperty("loginUser")]
    public string LoginUser { get; set; } = "";

    [JsonProperty("loginPassword")]
    public string LoginPassword { get; set; } = "";

    [JsonProperty("ipAddress")]
    public string IpAddress { get; set; } = "";

    [JsonProperty("provinceType")]
    public int ProvinceType { get; set; }
}

public class ProductUrlMonitorEntryDetailModelValidator : AbstractValidator<ProductUrlMonitorEntryDetailModel>
{
    public ProductUrlMonitorEntryDetailModelValidator()
    {
        RuleFor(e => e.ProductUrlMonitorId).NotNull().WithMessage("网站监测不能为空!");
        RuleFor(e => e.ProductUrlMonitorId).NotEmpty().WithMessage("网站监测不能为空!");

        RuleFor(e => e.LoginUser).NotNull().WithMessage("账号不能为空!");
        RuleFor(e => e.LoginUser).NotEmpty().WithMessage("账号不能为空!");
        RuleFor(e => e.LoginUser).MaximumLength(128).WithMessage("账号最大长度不能超过128!");

        RuleFor(e => e.LoginPassword).NotNull().WithMessage("密码不能为空!");
        RuleFor(e => e.LoginPassword).NotEmpty().WithMessage("密码不能为空!");
        RuleFor(e => e.LoginPassword).MaximumLength(128).WithMessage("密码最大长度不能超过128!");

        RuleFor(e => e.IpAddress).NotNull().WithMessage("使用地址不能为空!");
        RuleFor(e => e.IpAddress).NotEmpty().WithMessage("使用地址不能为空!");
        RuleFor(e => e.IpAddress).MaximumLength(32).WithMessage("使用地址最大长度不能超过32!");

        RuleFor(e => e.ProvinceType).NotNull().WithMessage("省份不能为空!");
        RuleFor(e => e.ProvinceType).NotEmpty().WithMessage("省份不能为空!");
    }
}