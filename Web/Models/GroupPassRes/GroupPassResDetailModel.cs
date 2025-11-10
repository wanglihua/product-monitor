
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.GroupPassRes;

public class GroupPassResDetailModel
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("groupId")]
    public long GroupId { get; set; }

    [JsonProperty("passResId")]
    public long PassResId { get; set; }

}


public class GroupPassResDetailModelValidator : AbstractValidator<GroupPassResDetailModel>
{
    public GroupPassResDetailModelValidator()
    {
        RuleFor(e => e.GroupId).NotNull().WithMessage("群组不能为空!");
        RuleFor(e => e.GroupId).NotEmpty().WithMessage("群组不能为空!");

        RuleFor(e => e.PassResId).NotNull().WithMessage("资源不能为空!");
        RuleFor(e => e.PassResId).NotEmpty().WithMessage("资源不能为空!");

    }
}
