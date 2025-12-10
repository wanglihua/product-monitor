using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.UserProvince;

public class UserProvinceListParam : ListParam
{
    [JsonProperty("userNameQuery")]
    public string UserNameQuery { get; set; } = "";

    [JsonProperty("provinceType")]
    public int ProvinceType { get; set; }
}

public class UserProvinceListParamValidator : AbstractValidator<UserProvinceListParam>
{
    public UserProvinceListParamValidator()
    {
    }
}