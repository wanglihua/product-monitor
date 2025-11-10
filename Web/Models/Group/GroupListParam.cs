using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.Group;

public class GroupListParam : ListParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}

public class GroupListParamValidator : AbstractValidator<GroupListParam>
{
    public GroupListParamValidator()
    {
    }
}