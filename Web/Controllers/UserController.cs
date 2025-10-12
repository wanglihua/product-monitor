using Core.Auth;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Web.Controllers;

[Auth]
public class UserController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
}