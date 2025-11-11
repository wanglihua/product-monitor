using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.UserPassRes;

public class UserPassResListParam : ListParam
{
    [JsonProperty("userId")]
    public long UserId { get; set; }

    [JsonProperty("passResNameQuery")]
    public string PassResNameQuery { get; set; } = "";
}

public class UserPassResListParamValidator : AbstractValidator<UserPassResListParam>
{
    public UserPassResListParamValidator()
    {
        RuleFor(e => e.UserId).GreaterThan(0).WithMessage("用户Id不能为空!");
    }
}