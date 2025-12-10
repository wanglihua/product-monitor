
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.UserProvince;

public class UserProvinceDetailModel
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("userId")]
    public string UserId { get; set; }

    [JsonProperty("provinceType")]
    public string ProvinceType { get; set; }

}


public class UserProvinceDetailModelValidator : AbstractValidator<UserProvinceDetailModel>
{
    public UserProvinceDetailModelValidator()
    {
        RuleFor(e => e.UserId).NotNull().WithMessage("不能为空!");
        RuleFor(e => e.UserId).NotEmpty().WithMessage("不能为空!");

        RuleFor(e => e.ProvinceType).NotNull().WithMessage("不能为空!");
        RuleFor(e => e.ProvinceType).NotEmpty().WithMessage("不能为空!");

    }
}
