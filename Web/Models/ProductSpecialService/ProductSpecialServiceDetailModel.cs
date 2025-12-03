using Domain;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductSpecialService;

public class ProductSpecialServiceDetailModel
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

    [JsonProperty("projectTypeCode")]
    public int ProjectTypeCode { get; set; }

    [JsonProperty("serviceDate")]
    public string ServiceDate { get; set; } = "";

    [JsonProperty("serviceCount")]
    public int ServiceCount { get; set; }

    [JsonProperty("serviceUrl")]
    public string ServiceUrl { get; set; } = "";

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

public class ProductSpecialServiceDetailModelValidator : AbstractValidator<ProductSpecialServiceDetailModel>
{
    public ProductSpecialServiceDetailModelValidator()
    {
        RuleFor(e => e.CollegeId).NotNull().WithMessage("学校不能为空!");
        RuleFor(e => e.CollegeId).NotEmpty().WithMessage("学校不能为空!");

        RuleFor(e => e.ProductId).NotNull().WithMessage("产品不能为空!");
        RuleFor(e => e.ProductId).NotEmpty().WithMessage("产品不能为空!");

        RuleFor(e => e.ProjectTypeCode)
            .Must(p => ProjectType.GetById(p) != ProjectType.None)
            .WithMessage("项目类型不能为空!");

        RuleFor(e => e.ServiceDate).NotNull().WithMessage("售后时间不能为空!");
        RuleFor(e => e.ServiceDate).NotEmpty().WithMessage("售后时间不能为空!");
        RuleFor(e => e.ServiceDate)
            .Must(p => DateTime.TryParse(p, out var serviceDate))
            .WithMessage("售后时间格式不正确!");

        RuleFor(e => e.ServiceUrl).NotNull().WithMessage("售后网址不能为空!");
        RuleFor(e => e.ServiceUrl).NotEmpty().WithMessage("售后网址不能为空!");
        RuleFor(e => e.ServiceUrl).MaximumLength(256).WithMessage("售后网址最大长度不能超过256!");

        RuleFor(e => e.LoginUser).NotNull().WithMessage("售后账号不能为空!");
        RuleFor(e => e.LoginUser).NotEmpty().WithMessage("售后账号不能为空!");
        RuleFor(e => e.LoginUser).MaximumLength(128).WithMessage("售后账号最大长度不能超过128!");

        RuleFor(e => e.LoginPassword).NotNull().WithMessage("售后密码不能为空!");
        RuleFor(e => e.LoginPassword).NotEmpty().WithMessage("售后密码不能为空!");
        RuleFor(e => e.LoginPassword).MaximumLength(128).WithMessage("售后密码最大长度不能超过128!");

        RuleFor(e => e.ContactTeacher).NotNull().WithMessage("联系老师不能为空!");
        RuleFor(e => e.ContactTeacher).NotEmpty().WithMessage("联系老师不能为空!");
        RuleFor(e => e.ContactTeacher).MaximumLength(32).WithMessage("联系老师最大长度不能超过32!");

        RuleFor(e => e.ContactMobile).NotNull().WithMessage("联系电话不能为空!");
        RuleFor(e => e.ContactMobile).NotEmpty().WithMessage("联系电话不能为空!");
        RuleFor(e => e.ContactMobile).MaximumLength(32).WithMessage("联系电话最大长度不能超过32!");

        RuleFor(e => e.Description).NotNull().WithMessage("售后问题不能为空!");
        RuleFor(e => e.Description).NotEmpty().WithMessage("售后问题不能为空!");
        RuleFor(e => e.Description).MaximumLength(512).WithMessage("售后问题最大长度不能超过512!");
    }
}