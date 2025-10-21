using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductSale;

public class ProductSaleDetailModel
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("collegeId")]
    public long CollegeId { get; set; }

    [JsonProperty("productId")]
    public long ProductId { get; set; }

    [JsonProperty("saleTimes")]
    public int SaleTimes { get; set; }

    [JsonProperty("installTimes")]
    public int InstallTimes { get; set; }

    [JsonProperty("updateTimes")]
    public int UpdateTimes { get; set; }

    [JsonProperty("serviceTimes")]
    public int ServiceTimes { get; set; }

    [JsonProperty("version")]
    public string Version { get; set; } = "";

    [JsonProperty("saleDate")]
    public string SaleDate { get; set; } = "";

    [JsonProperty("serviceExpireDate")]
    public string ServiceExpireDate { get; set; } = "";

    [JsonProperty("description")]
    public string Description { get; set; } = "";
}

public class ProductSaleDetailModelValidator : AbstractValidator<ProductSaleDetailModel>
{
    public ProductSaleDetailModelValidator()
    {
        RuleFor(e => e.CollegeId).NotNull().WithMessage("学校不能为空!");
        RuleFor(e => e.CollegeId).NotEmpty().WithMessage("学校不能为空!");

        RuleFor(e => e.ProductId).NotNull().WithMessage("产品不能为空!");
        RuleFor(e => e.ProductId).NotEmpty().WithMessage("产品不能为空!");

        RuleFor(e => e.SaleTimes).NotNull().WithMessage("销售次数不能为空!");
        RuleFor(e => e.SaleTimes).NotEmpty().WithMessage("销售次数不能为空!");

        RuleFor(e => e.InstallTimes).NotNull().WithMessage("安装次数不能为空!");
        RuleFor(e => e.InstallTimes).NotEmpty().WithMessage("安装次数不能为空!");

        RuleFor(e => e.UpdateTimes).NotNull().WithMessage("更新次数不能为空!");
        RuleFor(e => e.UpdateTimes).NotEmpty().WithMessage("更新次数不能为空!");

        RuleFor(e => e.ServiceTimes).NotNull().WithMessage("售后次数不能为空!");
        RuleFor(e => e.ServiceTimes).NotEmpty().WithMessage("售后次数不能为空!");

        RuleFor(e => e.Version).NotNull().WithMessage("版本号不能为空!");
        RuleFor(e => e.Version).NotEmpty().WithMessage("版本号不能为空!");
        RuleFor(e => e.Version).MaximumLength(32).WithMessage("版本号最大长度不能超过32!");

        RuleFor(e => e.SaleDate).NotNull().WithMessage("销售时间不能为空!");
        RuleFor(e => e.SaleDate).NotEmpty().WithMessage("销售时间不能为空!");
        RuleFor(e => e.SaleDate)
            .Must(p => DateTime.TryParse(p, out var saleDate))
            .WithMessage("销售时间格式不正确!");

        RuleFor(e => e.ServiceExpireDate).NotNull().WithMessage("服务到期时间不能为空!");
        RuleFor(e => e.ServiceExpireDate).NotEmpty().WithMessage("服务到期时间不能为空!");
        RuleFor(e => e.ServiceExpireDate)
            .Must(p => DateTime.TryParse(p, out var serviceExpireDate))
            .WithMessage("服务到期时间格式不正确!");

        RuleFor(e => e.Description).NotNull().WithMessage("备注不能为空!");
        RuleFor(e => e.Description).NotEmpty().WithMessage("备注不能为空!");
        RuleFor(e => e.Description).MaximumLength(512).WithMessage("备注最大长度不能超过512!");
    }
}