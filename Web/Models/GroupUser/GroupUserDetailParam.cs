using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.GroupUser;

public class GroupUserDetailParam
{
    [JsonProperty("groupId")]
    public long GroupId { get; set; }
}

public class GroupUserDetailParamValidator : AbstractValidator<GroupUserDetailParam>
{
    public GroupUserDetailParamValidator()
    {
        RuleFor(e => e.GroupId).GreaterThan(0).WithMessage("群组Id必须大于0!");
    }
}
