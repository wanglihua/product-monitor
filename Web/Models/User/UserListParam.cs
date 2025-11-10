using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.User;

public class UserListParam : ListParam
{
    [JsonProperty("loginNameQuery")]
    public string LoginNameQuery { get; set; } = "";

    [JsonProperty("userNameQuery")]
    public string UserNameQuery { get; set; } = "";
}

public class UserListParamValidator : AbstractValidator<UserListParam>
{
    public UserListParamValidator()
    {
    }
}