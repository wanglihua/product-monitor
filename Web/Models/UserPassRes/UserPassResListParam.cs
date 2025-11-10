using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.UserPassRes;

public class UserPassResListParam : ListParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}

public class UserPassResListParamValidator : AbstractValidator<UserPassResListParam>
{
    public UserPassResListParamValidator()
    {
    }
}