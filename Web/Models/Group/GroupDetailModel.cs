
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.Group;

public class GroupDetailModel
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = ""; 

}


public class GroupDetailModelValidator : AbstractValidator<GroupDetailModel>
{
    public GroupDetailModelValidator()
    {
        RuleFor(e => e.Name).NotNull().WithMessage("群组名称不能为空!");
        RuleFor(e => e.Name).NotEmpty().WithMessage("群组名称不能为空!");
        RuleFor(e => e.Name).MaximumLength(128).WithMessage("群组名称最大长度不能超过128!");

    }
}
