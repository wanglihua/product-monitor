using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.GroupUser;

public class GroupUserEmptyParam
{
    [JsonProperty("groupId")]
    public long GroupId { get; set; }
}

public class GroupUserEmptyParamValidator : AbstractValidator<GroupUserEmptyParam>
{
    public GroupUserEmptyParamValidator()
    {
        RuleFor(e => e.GroupId).GreaterThan(0).WithMessage("群组Id不能为空!");
    }
}