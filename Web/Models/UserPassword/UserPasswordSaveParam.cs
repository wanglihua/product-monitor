using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.UserPassword;

public class UserPasswordSaveParam
{
    [JsonProperty("oldPassword")]
    public string OldPassword { get; set; } = "";

    [JsonProperty("newPassword")]
    public string NewPassword { get; set; } = "";

    [JsonProperty("newPasswordAgain")]
    public string NewPasswordAgain { get; set; } = "";
}

public class UserPasswordSaveParamValidator : AbstractValidator<UserPasswordSaveParam>
{
    public UserPasswordSaveParamValidator()
    {
        RuleFor(e => e.OldPassword).NotEmpty().WithMessage("旧密码不能为空!");
        RuleFor(e => e.OldPassword).MinimumLength(6).WithMessage("密码名长度不能小于6！");
        RuleFor(e => e.OldPassword).MaximumLength(20).WithMessage("密码名长度不能超过20！");

        RuleFor(e => e.NewPassword).NotEmpty().WithMessage("新密码不能为空!");
        RuleFor(e => e.NewPassword).MinimumLength(6).WithMessage("新密码名长度不能小于6！");
        RuleFor(e => e.NewPassword).MaximumLength(20).WithMessage("新密码名长度不能超过20！");

        RuleFor(e => e.NewPasswordAgain).NotEmpty().WithMessage("新密码重输不能为空!");
        RuleFor(e => e.NewPasswordAgain).MinimumLength(6).WithMessage("新密码重输名长度不能小于6！");
        RuleFor(e => e.NewPasswordAgain).MaximumLength(20).WithMessage("新密码重输名长度不能超过20！");
    }
}