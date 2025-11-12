using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.AuthResult;

public class AuthResultListParam : ListParam
{
    [JsonProperty("loginNameQuery")]
    public string LoginNameQuery { get; set; } = "";

    [JsonProperty("userNameQuery")]
    public string UserNameQuery { get; set; } = "";

    [JsonProperty("passResCateNameQuery")]
    public string PassResCateNameQuery { get; set; } = "";

    [JsonProperty("passResNameQuery")]
    public string PassResNameQuery { get; set; } = "";

    [JsonProperty("groupNameQuery")]
    public string GroupNameQuery { get; set; } = "";
}

public class AuthResultListParamValidator : AbstractValidator<AuthResultListParam>
{
    public AuthResultListParamValidator()
    {
    }
}