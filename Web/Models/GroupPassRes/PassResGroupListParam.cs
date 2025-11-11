using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.GroupPassRes;

public class PassResGroupListParam : ListParam
{
    [JsonProperty("passResId")]
    public long PassResId { get; set; }

    [JsonProperty("groupNameQuery")]
    public string GroupNameQuery { get; set; } = "";
}

public class PassResGroupListParamValidator : AbstractValidator<PassResGroupListParam>
{
    public PassResGroupListParamValidator()
    {
        RuleFor(e => e.PassResId).GreaterThan(0).WithMessage("授权资源Id不能为空!");
    }
}