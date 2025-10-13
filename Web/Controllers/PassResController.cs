using Core.Auth;
using Domain.PassRes;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Auth(PassRes.BasicSetting.PassRes)]
public class PassResController : Controller
{
}