using System.Security.Cryptography;
using System.Text;

namespace Core.Util;

public class HashUtil
{
    public static string GetMD5(string clearText)
    {
        using var md5 = MD5.Create();

        byte[] hashData = Encoding.UTF8.GetBytes(clearText);
        hashData = md5.ComputeHash(hashData);

        var sb = new StringBuilder();
        foreach (byte b in hashData)
        {
            sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }
    
    public static string GetHmacSHA1(string encryptText, string encryptKey)
    {
        var hmacSha1 = new HMACSHA1(Encoding.UTF8.GetBytes(encryptKey));
        byte[] hashData = hmacSha1.ComputeHash(Encoding.UTF8.GetBytes(encryptText));

        var sb = new StringBuilder();
        foreach (byte b in hashData)
        {
            sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }

    public static string GetHmacSHA1Base64(string encryptText, string encryptKey)
    {
        var hmacSha1 = new HMACSHA1(Encoding.UTF8.GetBytes(encryptKey));
        byte[] hashData = hmacSha1.ComputeHash(Encoding.UTF8.GetBytes(encryptText));
        return Convert.ToBase64String(hashData);
    }
}