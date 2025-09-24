using Core.Database;
using Core.Web;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace Web.Controllers;

public class TestController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;

    public TestController(Db db)
    {
        this.db = db;
    }

#if DEBUG

    [HttpGet]
    [Route("/test")]
    [Transactional]
    public IActionResult Index()
    {
        // var testList = db.Set<TestEntity>()
        //     .Where(e => e.Id > 0)
        //     .Take(100)
        //     .ToList();

        db.Set<WebSessionDataEntity>()
            .Join(db.Set<WebSessionEntity>(),
                sd => sd.WebSessionId,
                s => s.Id,
                (sd, s) => new { sd.Id, s.UpdateTime })
            .Where(e => e.UpdateTime < DateTime.Now.AddHours(-1))
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
        });
    }

#endif
}