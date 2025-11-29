using Core.Auth;
using Core.Database;
using Core.Web;
using Domain;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Web.Controllers;

[NoAuth]
public class TestController(
    Db db,
    PasswordService passwordService) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

#if DEBUG

    [HttpGet]
    [Route("/test")]
    [Transactional(true)]
    public IActionResult Index()
    {
        // var testList = db.Set<TestEntity>()
        //     .Where(e => e.Id > 0)
        //     .Take(100)
        //     .ToList();

        // db.Set<WebSessionDataEntity>()
        //     .Join(db.Set<WebSessionEntity>(),
        //         sd => sd.WebSessionId,
        //         s => s.Id,
        //         (sd, s) => new { sd.Id, s.UpdateTime })
        //     .Where(e => e.UpdateTime < DateTime.Now.AddHours(-1))
        //     .ToList();

        // string salt = passwordService.GenerateSalt();
        // string passwordEncrypt = passwordService.Encrypt("123456", salt);
        // var user = new UserEntity {
        //     LoginName = "admin",
        //     UserName = "管理员",
        //     Password = passwordEncrypt,
        //     Salt = salt,
        // };
        //
        // db.User.Add(user);
        //
        // db.SaveChanges();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
        });
    }

#endif
}