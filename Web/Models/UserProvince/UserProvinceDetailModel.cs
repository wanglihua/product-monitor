using Domain;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.UserProvince;

public class UserProvinceDetailModel
{
    [JsonProperty("userId")]
    public long UserId { get; set; }

    [JsonProperty("provinceType")]
    public int ProvinceType { get; set; }

    [JsonProperty("provinceTypeList")]
    public IList<Dictionary<string, object>> ProvinceTypeList => Domain.ProvinceType.GetList()
        .Select(e => new Dictionary<string, object> {
            ["value"] = e.Id,
            ["label"] = e.Name,
        })
        .ToList();
}

public class UserProvinceDetailModelValidator : AbstractValidator<UserProvinceDetailModel>
{
    public UserProvinceDetailModelValidator()
    {
        RuleFor(e => e.UserId).Must(p => p > 0).WithMessage("不能为空!");

        RuleFor(e => e.ProvinceType)
            .Must(p => ProvinceType.GetById(p) != ProvinceType.None)
            .WithMessage("不能为空!");
    }
}