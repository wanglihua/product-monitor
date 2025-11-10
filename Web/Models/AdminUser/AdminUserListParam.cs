using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.AdminUser;

public class AdminUserListParam : ListParam
{
    [JsonProperty("loginNameQuery")]
    public string LoginNameQuery { get; set; } = "";

    [JsonProperty("userNameQuery")]
    public string UserNameQuery { get; set; } = "";
}

public class AdminUserListParamValidator : AbstractValidator<AdminUserListParam>
{
    public AdminUserListParamValidator()
    {
    }
}