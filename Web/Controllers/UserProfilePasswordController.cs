using Core;
using Core.Auth;
using Core.Database;
using Core.Util;
using Domain;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Service;
using Web.Models.UserPassword;

namespace Web.Controllers;

[Auth]
public class UserProfilePasswordController(
    Db db,
    UserLoginService userLoginService,
    PasswordService passwordService) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    [HttpPost]
    [Route("/api/user/profile/password/save")]
    [Transactional]
    public IActionResult Save([FromBody] UserPasswordSaveParam param)
    {
        ValidationUtil.Validate<UserPasswordSaveParamValidator, UserPasswordSaveParam>(param);

        string oldPassword = param.OldPassword.Trim();
        string newPassword = param.NewPassword.Trim();
        string newPasswordAgain = param.NewPasswordAgain.Trim();

        if (newPassword != newPasswordAgain)
        {
            throw new ValidationException("新密码和新密码重输不匹配!");
        }

        long loginUserId = userLoginService.GetLoginUserId();

        var user = db.User.Find(loginUserId);
        if (user is null)
        {
            throw new ValidationException($"指定的用户#{loginUserId}不存在!");
        }

        if (user.Password != passwordService.Encrypt(oldPassword, user.Salt))
        {
            throw new ValidationException("请输入正确的旧密码!");
        }

        string salt = passwordService.GenerateSalt();
        string password = passwordService.Encrypt(newPassword, salt);

        user.Password = password;
        user.Salt = salt;

        db.SaveChanges();

        return Json(new Dictionary<string, object> {
            ["success"] = true,
            ["message"] = "密码修改成功!",
        });
    }
}