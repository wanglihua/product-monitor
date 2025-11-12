using Core.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using Service;

namespace Web.Filters;

public class AuthActionFilter : IActionFilter
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private readonly UserLoginService userLoginService;
    private readonly UserAuthService userAuthService;

    public AuthActionFilter(UserLoginService userLoginService, UserAuthService userAuthService)
    {
        this.userLoginService = userLoginService;
        this.userAuthService = userAuthService;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        log.Trace("AuthActionFilter.OnActionExecuting");

        var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
        if (actionDescriptor == null)
        {
            return;
        }

        // string controllerName = actionDescriptor.ControllerName;
        // string actionName = actionDescriptor.ActionName;

        #region 登录判断

        // 如果有 NoAuth 标记，就不用进行登录检查
        object[] controllerNoAuthAttributes = actionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(NoAuthAttribute), true);
        if (controllerNoAuthAttributes != null && controllerNoAuthAttributes.Length != 0)
        {
            return;
        }

        object[] actionNoAuthAttributes = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(NoAuthAttribute), true);
        if (actionNoAuthAttributes != null && actionNoAuthAttributes.Length != 0)
        {
            return;
        }

        long loginUserId = userLoginService.GetLoginUserId();

        // 如果 没有 NoAuth 标记， 并且用户没登录，就提示未登录错误
        if (loginUserId == 0)
        {
            ShowLogin(context);
            return;
        }

        #endregion

        // 下面的逻辑, 都假设已经登录了

        #region 权限判断

        // Controller 类上设置的 possResId 中有一个是已授权的，就是 Controller 级别已授权。就是这里的各个 passResId 是 "或 or" 的关系
        // Action 方法上设置的 passResId 中的有一个是已授权的，就是 Action 级别已授权。就是这里的各个 passResId 是 "或 or" 的关系
        // Controller 级别（如果有）已授权 和 Action 级别（如果有）已授权，同时满足，这个Action才能正常访问。就是 Controller级别 和 Action级别 是 "且 and" 的关系                    

        object[] controllerAuthAttributes = actionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(AuthAttribute), true);
        object[] actionAuthAttributes = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(AuthAttribute), true);

        bool isAdmin = userLoginService.IsAdmin(loginUserId);

        // 如果当前是管理员登录，接下来的权限判断也就用不着了
        if (isAdmin == true)
        {
            return;
        }

        // 下面的逻辑, 都假设不是管理员登录

        // 两个循环中, 只做阻拦操作, 不做放行操作. 这样 controller 和 action 上的设置就是是 and 关系

        foreach (object authAttributeObj in controllerAuthAttributes)
        {
            if (authAttributeObj is AuthAttribute authAttribute)
            {
                // 判断是否是管理员登录
                if ((authAttribute.AdminOnly /* && isAdmin == false */)) // isAdmin false的是不会走到这一步的，前面已经有判断
                {
                    ShowLogin(context);
                    return;
                }

                // 判断是否是有标记的权限
                // authAttribute.PassResIdList 中只要有一个授权了，就是可以访问标注的 Controller 或 Action 了
                if (authAttribute.PassResIdList.Count != 0 && userAuthService.UserHasOneOfPassRes(loginUserId, authAttribute.PassResIdList) == false)
                {
                    ShowLogin(context);
                    return;
                }
            }
        }

        foreach (object authAttributeObj in actionAuthAttributes)
        {
            if (authAttributeObj is AuthAttribute authAttribute)
            {
                // 判断是否是管理员登录
                if ((authAttribute.AdminOnly /* && isAdmin == false */)) // isAdmin false的是不会走到这一步的，前面已经有判断
                {
                    ShowLogin(context);
                    return;
                }

                // 判断是否是有标记的权限
                // authAttribute.PassResIdList 中只要有一个授权了，就是可以访问标注的 Controller 或 Action 了
                if (authAttribute.PassResIdList.Count != 0 && userAuthService.UserHasOneOfPassRes(loginUserId, authAttribute.PassResIdList) == false)
                {
                    ShowLogin(context);
                    return;
                }
            }
        }

        #endregion
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        log.Trace("AuthActionFilter.OnActionExecuted");
    }

    private void ShowLogin(ActionExecutingContext context)
    {
        context.Result = new JsonResult(new Dictionary<string, object>() {
            { "success", false },
            { "message", "用户未登录，或登录会话过期, 或权限不够，请重新登录！" }
        });
    }
}