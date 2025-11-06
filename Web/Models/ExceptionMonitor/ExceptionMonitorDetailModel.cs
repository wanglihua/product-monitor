using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ExceptionMonitor;

public class ExceptionMonitorDetailModel
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("productId")]
    public long ProductId { get; set; }

    [JsonProperty("ipAddress")]
    public string IpAddress { get; set; } = "";

    [JsonProperty("provinceType")]
    public int ProvinceType { get; set; }

    [JsonProperty("port")]
    public int Port { get; set; }
}

public class ExceptionMonitorDetailModelValidator : AbstractValidator<ExceptionMonitorDetailModel>
{
    public ExceptionMonitorDetailModelValidator()
    {
        RuleFor(e => e.ProductId).NotNull().WithMessage("产品不能为空!");
        RuleFor(e => e.ProductId).NotEmpty().WithMessage("产品不能为空!");

        RuleFor(e => e.IpAddress).NotNull().WithMessage("使用IP不能为空!");
        RuleFor(e => e.IpAddress).NotEmpty().WithMessage("使用IP不能为空!");
        RuleFor(e => e.IpAddress).MaximumLength(32).WithMessage("使用IP最大长度不能超过32!");

        RuleFor(e => e.ProvinceType).NotNull().WithMessage("使用省份不能为空!");
        RuleFor(e => e.ProvinceType).NotEmpty().WithMessage("使用省份不能为空!");

        RuleFor(e => e.Port).Must(p => p >= 0).WithMessage("使用端口不能为负数!");
    }
}