using Core;
using Core.Auth;
using Core.Database;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[NoAuth]
public class CaptchaController : Controller
{
    private readonly CaptchaService captchaService;

    public CaptchaController(CaptchaService captchaService)
    {
        this.captchaService = captchaService;
    }

    [HttpGet]
    [Route("/api/captcha")]
    [Transactional(false)]
    public IActionResult Index()
    {
        byte[] bytes = captchaService.Create();
        Response.Body.Dispose();
        return File(bytes, "image/png");
    }

    [HttpPost]
    [Route("/api/captcha/base64")]
    [Transactional(false)]
    public IActionResult Base64()
    {
        byte[] bytes = captchaService.Create();
        Response.Body.Dispose();

        return Json(new Dictionary<string, object>() {
            { "success", true },
            { "message", "success" },
            { "base64", $"data:image/png;base64,{Convert.ToBase64String(bytes)}" },
        });
    }
}