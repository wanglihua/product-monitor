using Core.Auth;
using Core.Database;
using Core.Web;
using Domain;
using Domain.PassRes;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Service;
using Web.Models.PassRes;

namespace Web.Controllers;

[Auth(PassRes.SystemManage.PassRes)]
public class PassResController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly PassResCateDataInitService passResCateDataInitService;
    private readonly PassResDataInitService passResDataInitService;
    private readonly ActionLogService actionLogService;

    public PassResController(
        Db db,
        UserLoginService userLoginService,
        PassResCateDataInitService passResCateDataInitService,
        PassResDataInitService passResDataInitService,
        ActionLogService actionLogService
    )
    {
        this.db = db;
        this.userLoginService = userLoginService;
        this.passResCateDataInitService = passResCateDataInitService;
        this.passResDataInitService = passResDataInitService;
        this.actionLogService = actionLogService;
    }

    [HttpPost]
    [Route("/api/pass/res/init/data")]
    [Transactional(true)]
    public IActionResult InitData()
    {
        long loginUserId = userLoginService.GetLoginUserId();

        passResCateDataInitService.InitData();
        passResDataInitService.InitData();

        actionLogService.Add(loginUserId, ActionLogType.PassResDataInit);

        return Json(new JsonResultData {
            Success = true,
            Message = "权限资源数据更新成功!",
        });
    }

    [HttpPost]
    [Route("/api/pass/res/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] PassResListParam param)
    {
        return Json(new JsonResultData {
            Success = true,
            Message = "获取数据成功!",
            Data = new Dictionary<string, object> {
            },
        });
    }
}