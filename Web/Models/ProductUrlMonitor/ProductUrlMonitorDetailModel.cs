using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductUrlMonitor;

public class ProductUrlMonitorDetailModel
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("collegeId")]
    public long CollegeId { get; set; }

    [JsonProperty("productId")]
    public long ProductId { get; set; }

    [JsonProperty("productUrlType")]
    public int ProductUrlType { get; set; }

    [JsonProperty("taskCount")]
    public int TaskCount { get; set; }

    [JsonProperty("teacherCount")]
    public int TeacherCount { get; set; }
}

public class ProductUrlMonitorDetailModelValidator : AbstractValidator<ProductUrlMonitorDetailModel>
{
    public ProductUrlMonitorDetailModelValidator()
    {
        RuleFor(e => e.CollegeId).Must(p => p > 0).WithMessage("学校不能为空!");
        RuleFor(e => e.ProductId).Must(p => p > 0).WithMessage("产品不能为空!");

        RuleFor(e => e.ProductUrlType).NotNull().WithMessage("分类不能为空!");
        RuleFor(e => e.ProductUrlType).NotEmpty().WithMessage("分类不能为空!");

        RuleFor(e => e.TaskCount).Must(p => p >= 0).WithMessage("任务数量不能为负数!");
        RuleFor(e => e.TeacherCount).Must(p => p >= 0).WithMessage("教师数量不能为负数!");
    }
}