using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.GroupPassRes;

public class GroupPassResRevokeParam
{
    [JsonProperty("groupId")]
    public long GroupId { get; set; }

    [JsonProperty("passResId")]
    public long PassResId { get; set; }
}

public class GroupPassResRevokeParamValidator : AbstractValidator<GroupPassResRevokeParam>
{
    public GroupPassResRevokeParamValidator()
    {
        RuleFor(e => e.GroupId).GreaterThan(0).WithMessage("群组不能为空!");
        RuleFor(e => e.PassResId).GreaterThan(0).WithMessage("资源不能为空!");
    }
}