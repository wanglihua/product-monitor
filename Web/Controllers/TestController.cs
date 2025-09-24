using Core.Database;
using Core.Web;
using Domain;
using Microsoft.AspNetCore.Mvc;
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

    [HttpGet]
    [Route("/test")]
    [Transactional]
    public IActionResult Index()
    {
        var testList = db.Set<TestEntity>()
            .Where(e => e.Id > 0)
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = testList,
        });
    }
}