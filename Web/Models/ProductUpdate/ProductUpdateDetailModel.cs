using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductUpdate;

public class ProductUpdateDetailModel
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

    [JsonProperty("versionUpdate")]
    public string VersionUpdate { get; set; } = "";

    [JsonProperty("updateCount")]
    public int UpdateCount { get; set; }

    [JsonProperty("updateUrl")]
    public string UpdateUrl { get; set; } = "";

    [JsonProperty("loginUser")]
    public string LoginUser { get; set; } = "";

    [JsonProperty("loginPassword")]
    public string LoginPassword { get; set; } = "";

    [JsonProperty("updateDate")]
    public string UpdateDate { get; set; } = "";

    [JsonProperty("salesManager")]
    public string SalesManager { get; set; } = "";

    [JsonProperty("contactTeacher")]
    public string ContactTeacher { get; set; } = "";

    [JsonProperty("contactMobile")]
    public string ContactMobile { get; set; } = "";

    [JsonProperty("description")]
    public string Description { get; set; } = "";
}

public class ProductUpdateDetailModelValidator : AbstractValidator<ProductUpdateDetailModel>
{
    public ProductUpdateDetailModelValidator()
    {
        RuleFor(e => e.CollegeId).NotNull().WithMessage("学校不能为空!");
        RuleFor(e => e.CollegeId).NotEmpty().WithMessage("学校不能为空!");

        RuleFor(e => e.ProductId).NotNull().WithMessage("产品不能为空!");
        RuleFor(e => e.ProductId).NotEmpty().WithMessage("产品不能为空!");

        RuleFor(e => e.VersionSale).NotNull().WithMessage("销售版本不能为空!");
        RuleFor(e => e.VersionSale).NotEmpty().WithMessage("销售版本不能为空!");
        RuleFor(e => e.VersionSale).MaximumLength(32).WithMessage("销售版本最大长度不能超过32!");

        RuleFor(e => e.VersionUpdate).NotNull().WithMessage("更新版本不能为空!");
        RuleFor(e => e.VersionUpdate).NotEmpty().WithMessage("更新版本不能为空!");
        RuleFor(e => e.VersionUpdate).MaximumLength(32).WithMessage("更新版本最大长度不能超过32!");

        RuleFor(e => e.UpdateCount).NotNull().WithMessage("更新次数不能为空!");
        RuleFor(e => e.UpdateCount).NotEmpty().WithMessage("更新次数不能为空!");

        RuleFor(e => e.UpdateUrl).NotNull().WithMessage("更新网址不能为空!");
        RuleFor(e => e.UpdateUrl).NotEmpty().WithMessage("更新网址不能为空!");
        RuleFor(e => e.UpdateUrl).MaximumLength(256).WithMessage("更新网址最大长度不能超过256!");

        RuleFor(e => e.LoginUser).NotNull().WithMessage("更新账号不能为空!");
        RuleFor(e => e.LoginUser).NotEmpty().WithMessage("更新账号不能为空!");
        RuleFor(e => e.LoginUser).MaximumLength(128).WithMessage("更新账号最大长度不能超过128!");

        RuleFor(e => e.LoginPassword).NotNull().WithMessage("更新密码不能为空!");
        RuleFor(e => e.LoginPassword).NotEmpty().WithMessage("更新密码不能为空!");
        RuleFor(e => e.LoginPassword).MaximumLength(128).WithMessage("更新密码最大长度不能超过128!");

        RuleFor(e => e.UpdateDate).NotNull().WithMessage("更新时间不能为空!");
        RuleFor(e => e.UpdateDate).NotEmpty().WithMessage("更新时间不能为空!");
        RuleFor(e => e.UpdateDate)
            .Must(p => DateTime.TryParse(p, out var updateDate))
            .WithMessage("更新时间格式不正确!");

        RuleFor(e => e.SalesManager).NotNull().WithMessage("业务经理不能为空!");
        RuleFor(e => e.SalesManager).NotEmpty().WithMessage("业务经理不能为空!");
        RuleFor(e => e.SalesManager).MaximumLength(32).WithMessage("业务经理最大长度不能超过32!");

        RuleFor(e => e.ContactTeacher).NotNull().WithMessage("联系老师不能为空!");
        RuleFor(e => e.ContactTeacher).NotEmpty().WithMessage("联系老师不能为空!");
        RuleFor(e => e.ContactTeacher).MaximumLength(32).WithMessage("联系老师最大长度不能超过32!");

        RuleFor(e => e.ContactMobile).NotNull().WithMessage("联系电话不能为空!");
        RuleFor(e => e.ContactMobile).NotEmpty().WithMessage("联系电话不能为空!");
        RuleFor(e => e.ContactMobile).MaximumLength(32).WithMessage("联系电话最大长度不能超过32!");

        RuleFor(e => e.Description).MaximumLength(512).WithMessage("其他备注最大长度不能超过512!");
    }
}