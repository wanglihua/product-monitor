using Core.Auth;
using Domain.PassRes;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Web.Controllers;

[Auth(PassRes.BasicSetting.User)]
public class UserController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
}