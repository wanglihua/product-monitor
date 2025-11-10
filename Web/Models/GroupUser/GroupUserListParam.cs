using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.GroupUser;

public class GroupUserListParam : ListParam
{
    [JsonProperty("groupId")]
    public long GroupId { get; set; }
}

public class GroupUserListParamValidator : AbstractValidator<GroupUserListParam>
{
    public GroupUserListParamValidator()
    {
        RuleFor(e => e.GroupId).GreaterThan(0).WithMessage("群组Id不能为空!");
    }
}