using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductInstallMonitor;

public class ProductInstallMonitorDetailModel
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("collegeId")]
    public long CollegeId { get; set; }

    [JsonProperty("productId")]
    public long ProductId { get; set; }

    [JsonProperty("provinceType")]
    public int ProvinceType { get; set; }

    [JsonProperty("cpuSerialNo")]
    public string CpuSerialNo { get; set; } = "";

    [JsonProperty("netMacAddress")]
    public string NetMacAddress { get; set; } = "";

    [JsonProperty("taskCount")]
    public int? TaskCount { get; set; }

    [JsonProperty("teacherCount")]
    public int? TeacherCount { get; set; }
}

public class ProductInstallMonitorDetailModelValidator : AbstractValidator<ProductInstallMonitorDetailModel>
{
    public ProductInstallMonitorDetailModelValidator()
    {
        RuleFor(e => e.CollegeId).NotNull().WithMessage("学校不能为空!");
        RuleFor(e => e.CollegeId).NotEmpty().WithMessage("学校不能为空!");

        RuleFor(e => e.ProductId).NotNull().WithMessage("产品不能为空!");
        RuleFor(e => e.ProductId).NotEmpty().WithMessage("产品不能为空!");

        RuleFor(e => e.ProvinceType).NotNull().WithMessage("省份不能为空!");
        RuleFor(e => e.ProvinceType).NotEmpty().WithMessage("省份不能为空!");

        RuleFor(e => e.CpuSerialNo).NotNull().WithMessage("CPU序列号不能为空!");
        RuleFor(e => e.CpuSerialNo).NotEmpty().WithMessage("CPU序列号不能为空!");
        RuleFor(e => e.CpuSerialNo).MaximumLength(128).WithMessage("CPU序列号最大长度不能超过128!");

        RuleFor(e => e.NetMacAddress).NotNull().WithMessage("网卡硬件地址不能为空!");
        RuleFor(e => e.NetMacAddress).NotEmpty().WithMessage("网卡硬件地址不能为空!");
        RuleFor(e => e.NetMacAddress).MaximumLength(128).WithMessage("网卡硬件地址最大长度不能超过128!");

        RuleFor(e => e.TaskCount).NotNull().WithMessage("任务数量不能为空!");
        RuleFor(e => e.TaskCount).NotEmpty().WithMessage("任务数量不能为空!");

        RuleFor(e => e.TeacherCount).NotNull().WithMessage("教师数量不能为空!");
        RuleFor(e => e.TeacherCount).NotEmpty().WithMessage("教师数量不能为空!");
    }
}