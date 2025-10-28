using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductRentEntry;

public class ProductRentEntryDetailModel
{
    [JsonProperty("productRentEntryId")]
    public long ProductRentEntryId { get; set; }

    [JsonProperty("productRentId")]
    public long ProductRentId { get; set; }

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

    [JsonProperty("description")]
    public string Description { get; set; } = "";
}

public class ProductRentEntryDetailModelValidator : AbstractValidator<ProductRentEntryDetailModel>
{
    public ProductRentEntryDetailModelValidator()
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

        RuleFor(e => e.Description).MaximumLength(512).WithMessage("备注最大长度不能超过512!");
    }
}