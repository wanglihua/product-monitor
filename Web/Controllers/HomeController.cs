using Core.Auth;
using Core.Database;
using Domain;
using Domain.PassRes;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Service;
using Web.Models.Home;

namespace Web.Controllers;

[Auth]
public class HomeController : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly Db db;
    private readonly UserLoginService userLoginService;
    private readonly UserAuthService userAuthService;

    public HomeController(
        Db db,
        UserLoginService userLoginService,
        UserAuthService userAuthService
    )
    {
        this.db = db;
        this.userLoginService = userLoginService;
        this.userAuthService = userAuthService;
    }

    [HttpGet]
    [Route("/home/data")]
    [Transactional(false)]
    public IActionResult HomeData()
    {
        return Json(new Dictionary<string, object> {
            ["success"] = true,
            ["message"] = "success"
        });
    }

    [HttpGet]
    [Route("/home/nav/menu/auth/data")]
    [Transactional(false)]
    public IActionResult NavMenuAuthData()
    {
        long loginUserId = userLoginService.GetLoginUserId();

        var authResultList = userAuthService.GetAuthResultList();

        var homeNavMenuData = new HomeNavMenuAuthData {
            DataMonitor = authResultList.Any(e => e.UserId == loginUserId && PassRes.DataMonitor.All.Contains(e.PassResId)),
            
            ProductInstallMonitor = authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.DataMonitor.ProductInstallMonitor),
            ProductUrlMonitor = authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.DataMonitor.ProductUrlMonitor),
            ExceptionMonitor = authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.DataMonitor.ExceptionMonitor),
            
            ProjectManage = authResultList.Any(e => e.UserId == loginUserId && PassRes.ProjectManage.All.Contains(e.PassResId)),
            
            ProductSale = authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.ProjectManage.ProductSale),
            ProductRent = authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.ProjectManage.ProductRent),
            ProductEval = authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.ProjectManage.ProductEval),
            ProjectAnalyze = authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.ProjectManage.ProjectAnalyze),
            
            ProductService = authResultList.Any(e => e.UserId == loginUserId && PassRes.ProductService.All.Contains(e.PassResId)),
            
            ProductInstall = authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.ProductService.ProductInstall),
            ProductUpdate = authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.ProductService.ProductUpdate),
            ProductSaleService = authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.ProductService.ProductSaleService),
            ProductSpecialService = authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.ProductService.ProductSpecialService),
            ProductServiceAnalyze = authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.ProductService.ProductServiceAnalyze),
            
            BasicSetting = authResultList.Any(e => e.UserId == loginUserId && PassRes.BasicSetting.All.Contains(e.PassResId)),
            
            Product = authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.BasicSetting.Product),
            College = authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.BasicSetting.College),
            ProductUrl = authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.BasicSetting.ProductUrl),
            
            SystemManage = authResultList.Any(e => e.UserId == loginUserId && PassRes.SystemManage.All.Contains(e.PassResId)),
            
            User = authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.SystemManage.User),
            Group = authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.SystemManage.Group),
            PassRes = authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.SystemManage.PassRes),
            ActionLog = authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.SystemManage.ActionLog),
        };

        return Json(new Dictionary<string, object> {
            ["success"] = true,
            ["message"] = "success",
            ["data"] = homeNavMenuData,
        });
    }
}