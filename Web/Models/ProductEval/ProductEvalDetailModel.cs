using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductEval;

public class ProductEvalDetailModel
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("collegeId")]
    public long CollegeId { get; set; }

    [JsonProperty("productId")]
    public long ProductId { get; set; }

    [JsonProperty("evalTimes")]
    public int EvalTimes { get; set; }

    [JsonProperty("serviceTimes")]
    public int ServiceTimes { get; set; }

    [JsonProperty("totalDays")]
    public int TotalDays { get; set; }

    [JsonProperty("remainDays")]
    public int RemainDays { get; set; }

    [JsonProperty("beginDate")]
    public string BeginDate { get; set; } = "";

    [JsonProperty("endDate")]
    public string EndDate { get; set; } = "";

    [JsonProperty("description")]
    public string Description { get; set; } = "";
}

public class ProductEvalDetailModelValidator : AbstractValidator<ProductEvalDetailModel>
{
    public ProductEvalDetailModelValidator()
    {
        RuleFor(e => e.CollegeId).NotNull().WithMessage("学校不能为空!");
        RuleFor(e => e.CollegeId).NotEmpty().WithMessage("学校不能为空!");

        RuleFor(e => e.ProductId).NotNull().WithMessage("产品不能为空!");
        RuleFor(e => e.ProductId).NotEmpty().WithMessage("产品不能为空!");

        RuleFor(e => e.EvalTimes).NotNull().WithMessage("试用次数不能为空!");
        RuleFor(e => e.EvalTimes).NotEmpty().WithMessage("试用次数不能为空!");

        RuleFor(e => e.ServiceTimes).NotNull().WithMessage("销售次数不能为空!");
        RuleFor(e => e.ServiceTimes).NotEmpty().WithMessage("销售次数不能为空!");

        RuleFor(e => e.TotalDays).NotNull().WithMessage("累计天数不能为空!");
        RuleFor(e => e.TotalDays).NotEmpty().WithMessage("累计天数不能为空!");

        RuleFor(e => e.RemainDays).NotNull().WithMessage("剩余天数不能为空!");
        RuleFor(e => e.RemainDays).NotEmpty().WithMessage("剩余天数不能为空!");

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