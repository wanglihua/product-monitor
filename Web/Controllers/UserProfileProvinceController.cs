using Core.Auth;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Web.Controllers;

[Auth]
public class UserProfileProvinceController(
    UserLoginService userLoginService,
    UserProvinceService userProvinceService) : Controller
{
    [HttpPost]
    [Route("/api/user/profile/province")]
    public IActionResult Province()
    {
        long loginUserId = userLoginService.GetLoginUserId();
        bool isAdminLogin = userLoginService.IsAdmin(loginUserId);

        IList<string> provinceList = new List<string>();
        if (isAdminLogin == true)
        {
            provinceList.Add("管理员用户可查看所有院校相关数据");
        }

        if (isAdminLogin == false)
        {
            var userProvinceList = userProvinceService.GetUserProvinceList(loginUserId);
            userProvinceList = userProvinceList.OrderBy(e => e.Id).ToList();
            foreach (var item in userProvinceList)
            {
                provinceList.Add(item.Name);
            }
        }

        if (provinceList.Count == 0)
        {
            provinceList.Add("当前不能查看任何院校数据相关，请联系管理员");
        }

        return Json(new Dictionary<string, object> {
            ["success"] = true,
            ["message"] = "success",
            ["provinceList"] = provinceList,
        });
    }
}