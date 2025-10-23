using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductInstall;

public class ProductInstallDetailModel
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("collegeId")]
    public long CollegeId { get; set; }

    [JsonProperty("collegeName")]
    public string CollegeName { get; set; } = "";

    [JsonProperty("productId")]
    public long ProductId { get; set; }

    [JsonProperty("productName")]
    public string ProductName { get; set; } = "";

    [JsonProperty("versionSale")]
    public string VersionSale { get; set; } = "";

    [JsonProperty("versionInstall")]
    public string VersionInstall { get; set; } = "";

    [JsonProperty("installDate")]
    public string InstallDate { get; set; } = "";

    [JsonProperty("updateUrl")]
    public string UpdateUrl { get; set; } = "";

    [JsonProperty("loginUser")]
    public string LoginUser { get; set; } = "";

    [JsonProperty("loginPassword")]
    public string LoginPassword { get; set; } = "";

    [JsonProperty("contactTeacher")]
    public string ContactTeacher { get; set; } = "";

    [JsonProperty("contactMobile")]
    public string ContactMobile { get; set; } = "";

    [JsonProperty("description")]
    public string Description { get; set; } = "";
}

public class ProductInstallDetailModelValidator : AbstractValidator<ProductInstallDetailModel>
{
    public ProductInstallDetailModelValidator()
    {
        RuleFor(e => e.CollegeId).NotNull().WithMessage("学校不能为空!");
        RuleFor(e => e.CollegeId).NotEmpty().WithMessage("学校不能为空!");

        RuleFor(e => e.ProductId).NotNull().WithMessage("产品不能为空!");
        RuleFor(e => e.ProductId).NotEmpty().WithMessage("产品不能为空!");

        RuleFor(e => e.VersionSale).NotNull().WithMessage("销售版本不能为空!");
        RuleFor(e => e.VersionSale).NotEmpty().WithMessage("销售版本不能为空!");
        RuleFor(e => e.VersionSale).MaximumLength(32).WithMessage("销售版本最大长度不能超过32!");

        RuleFor(e => e.VersionInstall).NotNull().WithMessage("安装版本不能为空!");
        RuleFor(e => e.VersionInstall).NotEmpty().WithMessage("安装版本不能为空!");
        RuleFor(e => e.VersionInstall).MaximumLength(32).WithMessage("安装版本最大长度不能超过32!");

        RuleFor(e => e.InstallDate).NotNull().WithMessage("安装时间不能为空!");
        RuleFor(e => e.InstallDate).NotEmpty().WithMessage("安装时间不能为空!");
        RuleFor(e => e.InstallDate)
            .Must(p => DateTime.TryParse(p, out var installDate))
            .WithMessage("安装时间格式不正确!");

        RuleFor(e => e.UpdateUrl).NotNull().WithMessage("更新网址不能为空!");
        RuleFor(e => e.UpdateUrl).NotEmpty().WithMessage("更新网址不能为空!");
        RuleFor(e => e.UpdateUrl).MaximumLength(256).WithMessage("更新网址最大长度不能超过256!");

        RuleFor(e => e.LoginUser).NotNull().WithMessage("系统账号不能为空!");
        RuleFor(e => e.LoginUser).NotEmpty().WithMessage("系统账号不能为空!");
        RuleFor(e => e.LoginUser).MaximumLength(128).WithMessage("系统账号最大长度不能超过128!");

        RuleFor(e => e.LoginPassword).NotNull().WithMessage("系统密码不能为空!");
        RuleFor(e => e.LoginPassword).NotEmpty().WithMessage("系统密码不能为空!");
        RuleFor(e => e.LoginPassword).MaximumLength(128).WithMessage("系统密码最大长度不能超过128!");

        RuleFor(e => e.ContactTeacher).NotNull().WithMessage("联系教师不能为空!");
        RuleFor(e => e.ContactTeacher).NotEmpty().WithMessage("联系教师不能为空!");
        RuleFor(e => e.ContactTeacher).MaximumLength(32).WithMessage("联系教师最大长度不能超过32!");

        RuleFor(e => e.ContactMobile).NotNull().WithMessage("联系电话不能为空!");
        RuleFor(e => e.ContactMobile).NotEmpty().WithMessage("联系电话不能为空!");
        RuleFor(e => e.ContactMobile).MaximumLength(32).WithMessage("联系电话最大长度不能超过32!");

        RuleFor(e => e.Description).MaximumLength(512).WithMessage("其他备注最大长度不能超过512!");
    }
}