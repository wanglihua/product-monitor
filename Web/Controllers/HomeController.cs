using Core.Auth;
using Core.Database;
using Domain.PassRes;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Service;
using Web.Models.Home;

namespace Web.Controllers;

[Auth]
public class HomeController(
    UserLoginService userLoginService,
    UserAuthService userAuthService) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    // 可用于查看 或 判断 后台服务是否正常运行, 是否可连通
    [HttpGet, HttpPost]
    [Route("/api/home")]
    [Transactional(false)]
    public IActionResult Index()
    {
        return Json(new Dictionary<string, object> {
            ["success"] = true,
            ["message"] = ""
        });
    }

    [HttpPost]
    [Route("/api/home/data")]
    [Transactional(false)]
    public IActionResult HomeData()
    {
        return Json(new Dictionary<string, object> {
            ["success"] = true,
            ["message"] = "success"
        });
    }

    [HttpPost]
    [Route("/api/home/nav/menu/auth/data")]
    [Transactional(false)]
    public IActionResult NavMenuAuthData()
    {
        long loginUserId = userLoginService.GetLoginUserId();
        bool isAdmin = userLoginService.IsAdmin(loginUserId);

        var authResultList = userAuthService.GetAuthResultList();

        var homeNavMenuData = new HomeNavMenuAuthData {
            Admin = isAdmin,

            DataMonitor = isAdmin || authResultList.Any(e => e.UserId == loginUserId && PassRes.DataMonitor.All.Contains(e.PassResId)),

            ProductInstallMonitor = isAdmin || authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.DataMonitor.ProductInstallMonitor),
            ProductUrlMonitor = isAdmin || authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.DataMonitor.ProductUrlMonitor),
            ExceptionMonitor = isAdmin || authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.DataMonitor.ExceptionMonitor),

            ProductService = isAdmin || authResultList.Any(e => e.UserId == loginUserId && PassRes.ProductService.All.Contains(e.PassResId)),

            ProductInstall = isAdmin || authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.ProductService.ProductInstall),
            ProductUpdate = isAdmin || authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.ProductService.ProductUpdate),
            ProductSaleService = isAdmin || authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.ProductService.ProductSaleService),
            ProductSpecialService = isAdmin || authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.ProductService.ProductSpecialService),
            ProductServiceAnalyze = isAdmin || authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.ProductService.ProductServiceAnalyze),

            ProjectManage = isAdmin || authResultList.Any(e => e.UserId == loginUserId && PassRes.ProjectManage.All.Contains(e.PassResId)),

            ProductSale = isAdmin || authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.ProjectManage.ProductSale),
            ProductRent = isAdmin || authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.ProjectManage.ProductRent),
            ProductEval = isAdmin || authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.ProjectManage.ProductEval),
            ProjectAnalyze = isAdmin || authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.ProjectManage.ProjectAnalyze),

            BasicSetting = isAdmin || authResultList.Any(e => e.UserId == loginUserId && PassRes.BasicSetting.All.Contains(e.PassResId)),

            Product = isAdmin || authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.BasicSetting.Product),
            College = isAdmin || authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.BasicSetting.College),
            ProductUrl = isAdmin || authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.BasicSetting.ProductUrl),

            SystemManage = isAdmin || authResultList.Any(e => e.UserId == loginUserId && PassRes.SystemManage.All.Contains(e.PassResId)),

            User = isAdmin || authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.SystemManage.User),
            UserProvince = isAdmin || authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.SystemManage.UserProvince),
            Group = isAdmin || authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.SystemManage.Group),
            PassRes = isAdmin || authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.SystemManage.PassRes),
            ClientKey = isAdmin || authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.SystemManage.ClientKey),
            ActionLog = isAdmin || authResultList.Any(e => e.UserId == loginUserId && e.PassResId == PassRes.SystemManage.ActionLog),
        };

        return Json(new Dictionary<string, object> {
            ["success"] = true,
            ["message"] = "success",
            ["data"] = homeNavMenuData,
        });
    }
}