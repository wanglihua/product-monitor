using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductSaleEntry;

public class ProductSaleEntryDetailModel
{
    [JsonProperty("productSaleEntryId")]
    public long ProductSaleEntryId { get; set; }

    [JsonProperty("productSaleId")]
    public long ProductSaleId { get; set; }

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

    [JsonProperty("version")]
    public string Version { get; set; } = "";

    [JsonProperty("saleDate")]
    public string SaleDate { get; set; } = "";

    [JsonProperty("serviceExpireDate")]
    public string ServiceExpireDate { get; set; } = "";

    [JsonProperty("salesManager")]
    public string SalesManager { get; set; } = "";

    [JsonProperty("contactTeacher")]
    public string ContactTeacher { get; set; } = "";

    [JsonProperty("contactMobile")]
    public string ContactMobile { get; set; } = "";

    [JsonProperty("description")]
    public string Description { get; set; } = "";

    [JsonProperty("productVersionList", NullValueHandling = NullValueHandling.Ignore)]
    public IList<Dictionary<string, object>>? ProductVersionList { get; set; }
}

public class ProductSaleEntryDetailModelValidator : AbstractValidator<ProductSaleEntryDetailModel>
{
    public ProductSaleEntryDetailModelValidator()
    {
        //  这几个关系较复杂，放到 action 中
        // RuleFor(e => e.ProductSaleId).Must(p => p > 0).WithMessage("销售目录 不能为空!");
        // RuleFor(e => e.CollegeId).Must(p => p > 0).WithMessage("学校 不能为空!");
        // RuleFor(e => e.ProductId).Must(p => p > 0).WithMessage("产品 不能为空!");

        // RuleFor(e => e.Version).NotNull().WithMessage("版本号不能为空!");
        // RuleFor(e => e.Version).NotEmpty().WithMessage("版本号不能为空!");
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

        // RuleFor(e => e.SalesManager).NotNull().WithMessage("业务经理不能为空!");
        // RuleFor(e => e.SalesManager).NotEmpty().WithMessage("业务经理不能为空!");
        RuleFor(e => e.SalesManager).MaximumLength(32).WithMessage("业务经理最大长度不能超过32!");

        // RuleFor(e => e.ContactTeacher).NotNull().WithMessage("联系老师不能为空!");
        // RuleFor(e => e.ContactTeacher).NotEmpty().WithMessage("联系老师不能为空!");
        RuleFor(e => e.ContactTeacher).MaximumLength(32).WithMessage("联系老师最大长度不能超过32!");

        // RuleFor(e => e.ContactMobile).NotNull().WithMessage("联系电话不能为空!");
        // RuleFor(e => e.ContactMobile).NotEmpty().WithMessage("联系电话不能为空!");
        RuleFor(e => e.ContactMobile).MaximumLength(32).WithMessage("联系电话最大长度不能超过32!");

        RuleFor(e => e.Description).MaximumLength(512).WithMessage("备注最大长度不能超过512!");
    }
}