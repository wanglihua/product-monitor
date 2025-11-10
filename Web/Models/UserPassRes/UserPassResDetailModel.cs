using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.UserPassRes;

public class UserPassResDetailModel
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("userId")]
    public long UserId { get; set; }

    [JsonProperty("passResId")]
    public long PassResId { get; set; }
}

public class UserPassResDetailModelValidator : AbstractValidator<UserPassResDetailModel>
{
    public UserPassResDetailModelValidator()
    {
        RuleFor(e => e.UserId).NotNull().WithMessage("用户不能为空!");
        RuleFor(e => e.UserId).NotEmpty().WithMessage("用户不能为空!");

        RuleFor(e => e.PassResId).NotNull().WithMessage("授权资源不能为空!");
        RuleFor(e => e.PassResId).NotEmpty().WithMessage("授权资源不能为空!");
    }
}