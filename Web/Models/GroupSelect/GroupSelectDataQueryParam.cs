using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.GroupSelect;

public class GroupSelectDataQueryParam
{
    [JsonProperty("search")]
    public string Search { get; set; } = "";
}

public class GroupSelectDataQueryParamValidator : AbstractValidator<GroupSelectDataQueryParam>
{
    public GroupSelectDataQueryParamValidator()
    {
    }
}