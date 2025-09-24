using Core.Util;

namespace Core.Auth;

[Service(ServiceType.Scoped)]
public class PasswordService
{
    /// <summary>
    /// 现在的后台用户的密码加密方式
    /// </summary>
    /// <param name="clearPassword"></param>
    /// <param name="salt">salt</param>
    /// <returns></returns>
    public string Encrypt(string clearPassword, string salt)
    {
        return HashUtil.GetHmacSHA1(clearPassword + salt, salt).ToLower();
    }

    /// <summary>
    /// 随机生成用于密码加密的 salt
    /// </summary>
    /// <returns></returns>
    public string GenerateSalt()
    {
        return Guid.NewGuid().ToString("N").Substring(0, 10); // 取 10 位也够用了
    }
}