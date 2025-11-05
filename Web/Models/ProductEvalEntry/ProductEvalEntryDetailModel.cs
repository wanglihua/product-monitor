using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductEvalEntry;

public class ProductEvalEntryDetailModel
{
    [JsonProperty("productEvalEntryId")]
    public long ProductEvalEntryId { get; set; }

    [JsonProperty("productEvalId")]
    public long ProductEvalId { get; set; }

    [JsonProperty("collegeId")]
    public long CollegeId { get; set; }

    // 只用于显示
    [JsonProperty("collegeName")]
    public string CollegeName { get; set; } = "";

    [JsonProperty("productId")]
    public long ProductId { get; set; }

    // 只用于显示
    [JsonProperty("productName")]
    public string ProductName { get; set; } = "";

    [JsonProperty("totalDays")]
    public int TotalDays { get; set; }

    [JsonProperty("beginDate")]
    public string BeginDate { get; set; } = "";

    [JsonProperty("endDate")]
    public string EndDate { get; set; } = "";

    [JsonProperty("salesManager")]
    public string SalesManager { get; set; } = "";

    [JsonProperty("contactTeacher")]
    public string ContactTeacher { get; set; } = "";

    [JsonProperty("contactMobile")]
    public string ContactMobile { get; set; } = "";

    [JsonProperty("description")]
    public string Description { get; set; } = "";
}

public class ProductEvalEntryDetailModelValidator : AbstractValidator<ProductEvalEntryDetailModel>
{
    public ProductEvalEntryDetailModelValidator()
    {
        RuleFor(e => e.BeginDate).NotNull().WithMessage("开始时间不能为空!");
        RuleFor(e => e.BeginDate).NotEmpty().WithMessage("开始时间不能为空!");
        RuleFor(e => e.BeginDate)
            .Must(p => DateTime.TryParse(p, out var beginDate))
            .WithMessage("开始时间格式不正确!");

        RuleFor(e => e.EndDate).NotNull().WithMessage("结束时间不能为空!");
        RuleFor(e => e.EndDate).NotEmpty().WithMessage("结束时间不能为空!");
        RuleFor(e => e.EndDate)
            .Must(p => DateTime.TryParse(p, out var endDate))
            .WithMessage("结束时间格式不正确!");

        RuleFor(e => e.SalesManager).NotNull().WithMessage("业务经理不能为空!");
        RuleFor(e => e.SalesManager).NotEmpty().WithMessage("业务经理不能为空!");
        RuleFor(e => e.SalesManager).MaximumLength(32).WithMessage("业务经理最大长度不能超过32!");

        RuleFor(e => e.ContactTeacher).NotNull().WithMessage("联系老师不能为空!");
        RuleFor(e => e.ContactTeacher).NotEmpty().WithMessage("联系老师不能为空!");
        RuleFor(e => e.ContactTeacher).MaximumLength(32).WithMessage("联系老师最大长度不能超过32!");

        RuleFor(e => e.ContactMobile).NotNull().WithMessage("联系电话不能为空!");
        RuleFor(e => e.ContactMobile).NotEmpty().WithMessage("联系电话不能为空!");
        RuleFor(e => e.ContactMobile).MaximumLength(32).WithMessage("联系电话最大长度不能超过32!");

        RuleFor(e => e.Description).MaximumLength(512).WithMessage("备注最大长度不能超过512!");
    }
}