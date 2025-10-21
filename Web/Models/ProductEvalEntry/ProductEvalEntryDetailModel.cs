using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductEvalEntry;

public class ProductEvalEntryDetailModel
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("productEvalId")]
    public long ProductEvalId { get; set; }

    [JsonProperty("totalDays")]
    public int TotalDays { get; set; }

    [JsonProperty("beginDate")]
    public string BeginDate { get; set; } = "";

    [JsonProperty("endDate")]
    public string EndDate { get; set; } = "";

    [JsonProperty("description")]
    public string Description { get; set; } = "";
}

public class ProductEvalEntryDetailModelValidator : AbstractValidator<ProductEvalEntryDetailModel>
{
    public ProductEvalEntryDetailModelValidator()
    {
        RuleFor(e => e.ProductEvalId).NotNull().WithMessage("试用目录不能为空!");
        RuleFor(e => e.ProductEvalId).NotEmpty().WithMessage("试用目录不能为空!");

        RuleFor(e => e.TotalDays).NotNull().WithMessage("累计天数不能为空!");
        RuleFor(e => e.TotalDays).NotEmpty().WithMessage("累计天数不能为空!");

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

        RuleFor(e => e.Description).NotNull().WithMessage("备注不能为空!");
        RuleFor(e => e.Description).NotEmpty().WithMessage("备注不能为空!");
        RuleFor(e => e.Description).MaximumLength(512).WithMessage("备注最大长度不能超过512!");
    }
}