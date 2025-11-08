using Domain;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductInstallMonitorEntry;

public class ProductInstallMonitorEntryDetailModel
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("productInstallMonitorId")]
    public long ProductInstallMonitorId { get; set; }

    [JsonProperty("loginUser")]
    public string LoginUser { get; set; } = "";

    [JsonProperty("accountType")]
    public int AccountType { get; set; }

    [JsonProperty("ipAddress")]
    public string IpAddress { get; set; } = "";

    [JsonProperty("provinceType")]
    public int ProvinceType { get; set; }
}

public class ProductInstallMonitorEntryDetailModelValidator : AbstractValidator<ProductInstallMonitorEntryDetailModel>
{
    public ProductInstallMonitorEntryDetailModelValidator()
    {
        RuleFor(e => e.ProductInstallMonitorId).NotNull().WithMessage("安装监测不能为空!");
        RuleFor(e => e.ProductInstallMonitorId).NotEmpty().WithMessage("安装监测不能为空!");

        RuleFor(e => e.LoginUser).NotNull().WithMessage("使用账号不能为空!");
        RuleFor(e => e.LoginUser).NotEmpty().WithMessage("使用账号不能为空!");
        RuleFor(e => e.LoginUser).MaximumLength(128).WithMessage("使用账号最大长度不能超过128!");

        RuleFor(e => e.AccountType).Must(p => AccountType.GetById(p) != AccountType.None).WithMessage("账号分类不能为空!");

        RuleFor(e => e.IpAddress).NotNull().WithMessage("使用IP不能为空!");
        RuleFor(e => e.IpAddress).NotEmpty().WithMessage("使用IP不能为空!");
        RuleFor(e => e.IpAddress).MaximumLength(32).WithMessage("使用IP最大长度不能超过32!");

        RuleFor(e => e.ProvinceType).Must(p => ProvinceType.GetById(p) != ProvinceType.None).WithMessage("使用省份不能为空!");
    }
}