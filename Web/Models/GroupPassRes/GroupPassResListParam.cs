using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.GroupPassRes;

public class GroupPassResListParam : ListParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}

public class GroupPassResListParamValidator : AbstractValidator<GroupPassResListParam>
{
    public GroupPassResListParamValidator()
    {
    }
}