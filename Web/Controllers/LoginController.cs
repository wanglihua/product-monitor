using Core;
using Core.Auth;
using Core.Database;
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
    private readonly UserLoginRateLimitService userLoginRateLimitService;
    private readonly UserLoginFailCountService userLoginFailCountService;

    public LoginController(
        Db db,
        UserLoginService userLoginService,
        CaptchaService captchaService,
        UserLoginRateLimitService userLoginRateLimitService,
        UserLoginFailCountService userLoginFailCountService)
    {
        this.db = db;
        this.userLoginService = userLoginService;
        this.captchaService = captchaService;
        this.userLoginRateLimitService = userLoginRateLimitService;
        this.userLoginFailCountService = userLoginFailCountService;
    }

    [HttpPost]
    [Route("/api/login")]
    [Transactional(false)]
    public IActionResult Login([FromBody] LoginParam param)
    {
        ValidationUtil.Validate<LoginParamValidator, LoginParam>(param);

        string loginName = param.LoginName.Trim();
        string password = param.Password.Trim();
        string captcha = param.Captcha.Trim();

        bool shouldShowCaptcha = userLoginFailCountService.ShouldShowCaptcha(loginName);
        if (shouldShowCaptcha && string.IsNullOrWhiteSpace(captcha) == true)
        {
            // throw new ValidationException("验证码不能为空！");

            return Json(new Dictionary<string, object> {
                ["success"] = false,
                ["message"] = "验证码不能为空！",
                ["showCaptcha"] = true,
            });
        }

        // 验证码检查
        if ((shouldShowCaptcha || string.IsNullOrWhiteSpace(captcha) == false)
            && captchaService.CheckInputCode(captcha) == false)
        {
            // 如果发现验证码不对,应该马上把原来的验证码设为无效
            captchaService.MakeCurrentExpire();

            // throw new ValidationException("请输入正确的验证码！");

            return Json(new Dictionary<string, object> {
                ["success"] = false,
                ["message"] = "请输入正确的验证码！",
                ["showCaptcha"] = true,
            });
        }

        userLoginRateLimitService.CheckRateLimit(loginName);

        long loginedUserId = 0;

        try
        {
            Transactional.Wrap(db, () => {
                // 就这个部分放在数据库事务里面了
                // 这样前后，直接返回json不是抛出异常也不会影响事务
                loginedUserId = userLoginService.Login(loginName, password, Request);
            });
        }
        catch (Exception exp)
        {
            log.Error(exp);

            return Json(new Dictionary<string, object>() {
                ["success"] = false,
                ["message"] = exp.Message,
                ["showCaptcha"] = shouldShowCaptcha,
            });
        }

        if (loginedUserId <= 0)
        {
            // 如果发现用户名或者密码不对,应该马上把原来的验证码设为无效
            captchaService.MakeCurrentExpire();

            // 记录一次登录失败
            bool showCaptcha = userLoginFailCountService.Add(loginName);

            // throw new ValidationException("登录名不存在或密码错误！");

            return Json(new Dictionary<string, object>() {
                ["success"] = false,
                ["message"] = "登录名不存在或密码错误！",
                ["showCaptcha"] = showCaptcha,
            });
        }

        var user = db.Find<UserEntity>(loginedUserId);
        string userName = user?.UserName ?? string.Empty;

        return Json(new Dictionary<string, object>() {
            ["success"] = true,
            ["message"] = "登录成功！",
            ["showCaptcha"] = false,
            ["data"] = new Dictionary<string, object> {
                ["userId"] = loginedUserId,
                ["loginName"] = loginName,
                ["userName"] = userName,
            },
        });
    }

    [HttpPost]
    [Route("/api/logout")]
    [Transactional(true)]
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
    [Transactional(false)]
    public IActionResult UserLoginCheck()
    {
        long loginUserId = userLoginService.GetLoginUserId();

        bool logined = false;
        string loginName = "";
        string userName = "";

        if (loginUserId > 0)
        {
            var user = db.Set<UserEntity>().Find(loginUserId);
            if (user is not null)
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