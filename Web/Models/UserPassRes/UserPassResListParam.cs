using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.UserPassRes;

public class UserPassResListParam : ListParam
{
    [JsonProperty("passResId")]
    public long PassResId { get; set; }

    [JsonProperty("loginNameQuery")]
    public string LoginNameQuery { get; set; } = "";

    [JsonProperty("userNameQuery")]
    public string UserNameQuery { get; set; } = "";
}

public class UserPassResListParamValidator : AbstractValidator<UserPassResListParam>
{
    public UserPassResListParamValidator()
    {
        RuleFor(e => e.PassResId).GreaterThan(0).WithMessage("授权资源Id不能为空!");
    }
}