using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.Login;

public class LoginParam
{
    /// <summary>
    /// 登录名
    /// </summary>
    [JsonProperty("loginName")]
    public string LoginName { get; set; } = "";

    /// <summary>
    /// 密码
    /// </summary>
    [JsonProperty("password")]
    public string Password { get; set; } = "";

    /// <summary>
    /// 验证码
    /// </summary>
    [JsonProperty("captcha")]
    public string Captcha { get; set; } = "";
}

public class LoginParamValidator : AbstractValidator<LoginParam>
{
    public LoginParamValidator()
    {
        RuleFor(e => e.LoginName).NotNull().WithMessage("登录名不能为空！");
        RuleFor(e => e.LoginName).NotEmpty().WithMessage("登录名不能为空！");
        RuleFor(e => e.LoginName).MaximumLength(100).WithMessage("登录名长度不能超过100！");

        RuleFor(e => e.Password).NotNull().WithMessage("密码不能为空！");
        RuleFor(e => e.Password).NotEmpty().WithMessage("密码不能为空！");
        RuleFor(e => e.Password).MaximumLength(100).WithMessage("密码长度不能超过100！");

        RuleFor(e => e.Captcha).Must(p => {
            if (string.IsNullOrWhiteSpace(p) == false)
            {
                string captcha = p.Trim();
                if (captcha.Length != 4)
                {
                    return false;
                }
            }

            return true;
        }).WithMessage("验证码长度应为4！");
    }
}