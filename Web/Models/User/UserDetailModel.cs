using Domain;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.User;

public class UserDetailModel
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("loginName")]
    public string LoginName { get; set; } = "";

    [JsonProperty("userName")]
    public string UserName { get; set; } = "";

    [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
    public string Password { get; set; } = "";

    [JsonProperty("passwordAgain", NullValueHandling = NullValueHandling.Ignore)]
    public string PasswordAgain { get; set; } = "";

    [JsonProperty("statusId")]
    public int? StatusId { get; set; }

    [JsonProperty("userStatusList")]
    public IList<Dictionary<string, object>> UserStatusList => UserStatus.GetNormalList()
        .Select(e => new Dictionary<string, object> {
            ["value"] = e.Id,
            ["label"] = e.Name,
        })
        .ToList();
}

public class UserDetailModelCreateValidator : AbstractValidator<UserDetailModel>
{
    public UserDetailModelCreateValidator()
    {
        RuleFor(e => e.LoginName).NotNull().WithMessage("登录名不能为空！");
        RuleFor(e => e.LoginName).NotEmpty().WithMessage("登录名不能为空！");
        RuleFor(e => e.LoginName).MaximumLength(30).WithMessage("登录名长度不能超过30！");

        RuleFor(e => e.UserName).NotNull().WithMessage("用户名不能为空！");
        RuleFor(e => e.UserName).NotEmpty().WithMessage("用户名不能为空！");
        RuleFor(e => e.UserName).MaximumLength(30).WithMessage("用户名长度不能超过30！");

        RuleFor(e => e.Password).NotNull().WithMessage("密码不能为空！");
        RuleFor(e => e.Password).NotEmpty().WithMessage("密码不能为空！");
        RuleFor(e => e.Password).MinimumLength(6).WithMessage("密码名长度不能小于6！");
        RuleFor(e => e.Password).MaximumLength(20).WithMessage("密码名长度不能超过20！");

        RuleFor(e => e.PasswordAgain).NotNull().WithMessage("密码重输不能为空！");
        RuleFor(e => e.PasswordAgain).NotEmpty().WithMessage("密码重输不能为空！");
        RuleFor(e => e.PasswordAgain).MinimumLength(6).WithMessage("密码重输长度不能小于6！");
        RuleFor(e => e.PasswordAgain).MaximumLength(20).WithMessage("密码重输长度不能超过20！");

        RuleFor(e => e.PasswordAgain)
            .Must((e, p) => {
                if (string.IsNullOrWhiteSpace(e.Password) && string.IsNullOrWhiteSpace(e.PasswordAgain))
                {
                    return true;
                }

                return e.PasswordAgain == e.Password;
            })
            .WithMessage("密码和密码重输不相等！");

        RuleFor(e => e.StatusId).Must(p => p > 0 && UserStatus.GetById(p.Value) != UserStatus.None).WithMessage("状态不能为空！");
    }
}

public class UserDetailModelEditValidator : AbstractValidator<UserDetailModel>
{
    public UserDetailModelEditValidator()
    {
        RuleFor(e => e.Id).Must(p => p > 0).WithMessage("id 不能为空！");

        RuleFor(e => e.LoginName).NotNull().WithMessage("登录名不能为空！");
        RuleFor(e => e.LoginName).NotEmpty().WithMessage("登录名不能为空！");
        RuleFor(e => e.LoginName).MaximumLength(30).WithMessage("登录名长度不能超过30！");

        RuleFor(e => e.UserName).NotNull().WithMessage("用户名不能为空！");
        RuleFor(e => e.UserName).NotEmpty().WithMessage("用户名不能为空！");
        RuleFor(e => e.UserName).MaximumLength(30).WithMessage("用户名长度不能超过30！");

        RuleFor(e => e.Password)
            .Must((e, p) => {
                if (string.IsNullOrWhiteSpace(e.Password) == false && p.Length < 6)
                {
                    return false;
                }

                return true;
            })
            .WithMessage("密码名长度不能小于6！");

        RuleFor(e => e.Password)
            .Must((e, p) => {
                if (string.IsNullOrWhiteSpace(e.Password) == false && p.Length > 20)
                {
                    return false;
                }

                return true;
            })
            .WithMessage("密码名长度不能超过20！");

        RuleFor(e => e.PasswordAgain)
            .Must((e, p) => {
                if (string.IsNullOrWhiteSpace(e.PasswordAgain) == false && p.Length < 6)
                {
                    return false;
                }

                return true;
            })
            .WithMessage("密码重输长度不能小于6！");

        RuleFor(e => e.PasswordAgain)
            .Must((e, p) => {
                if (string.IsNullOrWhiteSpace(e.PasswordAgain) == false && p.Length > 20)
                {
                    return false;
                }

                return true;
            })
            .WithMessage("密码重输长度不能超过20！");

        RuleFor(e => e.PasswordAgain)
            .Must((e, p) => {
                // 其中有一个 不 为空的时候, 就比较
                if (string.IsNullOrWhiteSpace(e.Password) == false || string.IsNullOrWhiteSpace(e.PasswordAgain) == false)
                {
                    return e.PasswordAgain == e.Password;
                }

                return true;
            })
            .WithMessage("密码和密码重输不相等！");

        RuleFor(e => e.StatusId).Must(p => p > 0 && UserStatus.GetById(p.Value) != UserStatus.None).WithMessage("状态不能为空！");
    }
}