using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.UserProvince;

public class UserProvinceListParam : ListParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}

public class UserProvinceListParamValidator : AbstractValidator<UserProvinceListParam>
{
    public UserProvinceListParamValidator()
    {
    }
}