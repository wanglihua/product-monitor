using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.UserPassRes;

public class UserPassResRevokeParam
{
    [JsonProperty("userId")]
    public long UserId { get; set; }

    [JsonProperty("passResId")]
    public long PassResId { get; set; }
}

public class UserPassResRevokeParamValidator : AbstractValidator<UserPassResRevokeParam>
{
    public UserPassResRevokeParamValidator()
    {
        RuleFor(e => e.UserId).GreaterThan(0).WithMessage("用户不能为空!");
        RuleFor(e => e.PassResId).GreaterThan(0).WithMessage("授权资源不能为空!");
    }
}