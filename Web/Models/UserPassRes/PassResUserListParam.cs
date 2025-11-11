using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.UserPassRes;

public class PassResUserListParam : ListParam
{
    [JsonProperty("passResId")]
    public long PassResId { get; set; }

    [JsonProperty("loginNameQuery")]
    public string LoginNameQuery { get; set; } = "";

    [JsonProperty("userNameQuery")]
    public string UserNameQuery { get; set; } = "";
}

public class PassResUserListParamValidator : AbstractValidator<PassResUserListParam>
{
    public PassResUserListParamValidator()
    {
        RuleFor(e => e.PassResId).GreaterThan(0).WithMessage("授权资源Id不能为空!");
    }
}