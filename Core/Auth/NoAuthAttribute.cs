namespace Core.Auth;

/// <summary>
/// 标记 Controller 或者 Action 的无效登录授权
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class NoAuthAttribute : Attribute
{
}