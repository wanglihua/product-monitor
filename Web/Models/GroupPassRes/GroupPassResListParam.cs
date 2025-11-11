using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.GroupPassRes;

public class GroupPassResListParam : ListParam
{
    [JsonProperty("groupId")]
    public long GroupId { get; set; }

    // todo 可以加上个按资源类别查询

    [JsonProperty("passResNameQuery")]
    public string PassResNameQuery { get; set; } = "";
}

public class GroupPassResListParamValidator : AbstractValidator<GroupPassResListParam>
{
    public GroupPassResListParamValidator()
    {
        RuleFor(e => e.GroupId).GreaterThan(0).WithMessage("群组Id不能为空!");
    }
}