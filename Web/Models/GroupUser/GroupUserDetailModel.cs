using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.GroupUser;

public class GroupUserDetailModel
{
    [JsonProperty("groupId")]
    public long GroupId { get; set; }

    [JsonProperty("userId")]
    public long UserId { get; set; }
}

public class GroupUserDetailModelValidator : AbstractValidator<GroupUserDetailModel>
{
    public GroupUserDetailModelValidator()
    {
        RuleFor(e => e.GroupId).Must(p => p > 0).WithMessage("群组不能为空!");
        RuleFor(e => e.UserId).Must(p => p > 0).WithMessage("用户不能为空!");
    }
}