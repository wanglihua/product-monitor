using System.Drawing;
using Domain;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductUrl;

public class ProductUrlDetailModel
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = "";

    [JsonProperty("url")]
    public string Url { get; set; } = "";

    [JsonProperty("typeCode")]
    public int TypeCode { get; set; }

    [JsonProperty("loginUser")]
    public string LoginUser { get; set; } = "";

    [JsonProperty("loginPassword")]
    public string LoginPassword { get; set; } = "";

    [JsonProperty("description")]
    public string Description { get; set; } = "";

    [JsonProperty("productUrlTypeList")]
    public IList<Dictionary<string, object>> ProductUrlTypeList => ProductUrlType.GetList()
        .Select(e => new Dictionary<string, object> {
            ["value"] = e.Id,
            ["label"] = e.Name,
        })
        .ToList();
}

public class ProductUrlDetailModelValidator : AbstractValidator<ProductUrlDetailModel>
{
    public ProductUrlDetailModelValidator()
    {
        RuleFor(e => e.Name).NotNull().WithMessage("产品名称不能为空!");
        RuleFor(e => e.Name).NotEmpty().WithMessage("产品名称不能为空!");
        RuleFor(e => e.Name).MaximumLength(128).WithMessage("产品名称最大长度不能超过128!");

        RuleFor(e => e.Url).NotNull().WithMessage("网址不能为空!");
        RuleFor(e => e.Url).NotEmpty().WithMessage("网址不能为空!");
        RuleFor(e => e.Url).MaximumLength(256).WithMessage("网址最大长度不能超过256!");

        RuleFor(e => e.TypeCode).Must(p => ProductUrlType.GetById(p) != ProductUrlType.None).WithMessage("类型不能为空!");

        RuleFor(e => e.LoginUser).NotNull().WithMessage("账号不能为空!");
        RuleFor(e => e.LoginUser).NotEmpty().WithMessage("账号不能为空!");
        RuleFor(e => e.LoginUser).MaximumLength(128).WithMessage("账号最大长度不能超过128!");

        RuleFor(e => e.LoginPassword).NotNull().WithMessage("密码不能为空!");
        RuleFor(e => e.LoginPassword).NotEmpty().WithMessage("密码不能为空!");
        RuleFor(e => e.LoginPassword).MaximumLength(128).WithMessage("密码最大长度不能超过128!");

        RuleFor(e => e.Description).MaximumLength(512).WithMessage("备注最大长度不能超过128!");
    }
}