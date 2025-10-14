using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.AdminUser;

public class AdminUserAddSaveParam
{
    [JsonProperty("loginName")]
    public string LoginName { get; set; } = "";
}

public class AdminUserAddSaveParamValidator : AbstractValidator<AdminUserAddSaveParam>
{
    public AdminUserAddSaveParamValidator()
    {
        RuleFor(x => x.LoginName).NotNull().WithMessage("登录名不能为空！");
        RuleFor(x => x.LoginName).NotEmpty().WithMessage("登录名不能为空！");
    }
}