using Core.Auth;
using Domain;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Web.Controllers;

[Auth]
public class UserPasswordController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;

    public UserPasswordController(Db db)
    {
        this.db = db;
    }
}