using Core;
using Core.Auth;
using Core.Util;
using Core.Web;
using Domain;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Service;
using Web.Models.Login;

namespace Web.Controllers;

[NoAuth]
public class LoginController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly CaptchaService captchaService;

    public LoginController(
        Db db,
        UserLoginService userLoginService,
        CaptchaService captchaService)
    {
        this.db = db;
        this.userLoginService = userLoginService;
        this.captchaService = captchaService;
    }

    [HttpPost]
    [Route("/api/login")]
    public IActionResult Login([FromBody] LoginParam param)
    {
        ValidationUtil.Validate<LoginParamValidator, LoginParam>(param);

        string loginName = param.LoginName.Trim();
        string password = param.Password.Trim();
        string captcha = param.Captcha.Trim();

        // 验证码检查
        if (captchaService.CheckInputCode(captcha) == false)
        {
            captchaService.MakeCurrentExpire();
            throw new ValidationException("请输入正确的验证码！");
        }

        long loginedUserId = userLoginService.Login(loginName, password, Request);
        if (loginedUserId <= 0)
        {
            captchaService.MakeCurrentExpire();
            throw new ValidationException("登录名不存在或密码错误！");
        }

        var user = db.Find<UserEntity>(loginedUserId);
        string userName = user?.UserName ?? string.Empty;

        return Json(new JsonResultData {
            Success = true,
            Message = "登录成功！",
            Data = new Dictionary<string, object> {
                { "userId", loginedUserId },
                { "loginName", loginName },
                { "userName", userName },
            },
        });
    }

    [HttpPost]
    [Route("/api/logout")]
    public IActionResult Logout()
    {
        userLoginService.Logout();

        return Json(new JsonResultData {
            Success = true,
            Message = "注销成功！"
        });
    }

    [HttpPost]
    [Route("/api/login/check")]
    public IActionResult UserLoginCheck()
    {
        long loginUserId = userLoginService.GetLoginUserId();

        bool logined = false;
        string loginName = "";
        string userName = "";

        if (loginUserId > 0)
        {
            var user = db.Set<UserEntity>().Find(loginUserId);
            if (user != null)
            {
                logined = true;
                loginName = user.LoginName;
                userName = user.UserName;
            }
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                { "logined", logined },
                { "userId", loginUserId },
                { "loginName", loginName },
                { "userName", userName },
            },
        });
    }
}