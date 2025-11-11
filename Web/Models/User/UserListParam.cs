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

    // todo 需要的时候, 可以加上个按状态查询
}

public class UserListParamValidator : AbstractValidator<UserListParam>
{
    public UserListParamValidator()
    {
    }
}