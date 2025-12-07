using Core;
using Core.Util;
using Domain;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class ClientKeyService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private string salt => "dc2cfff6c7576a2bbbf4d85bd0c58fb6"; // jcjy md5

    public string GenerateNewKey()
    {
        var guid = Guid.CreateVersion7();

        var guidPartList = guid.ToString("D").Split("-").ToList();
        string hash = HashUtil.GetMD5(string.Concat(guidPartList.OrderByDescending(e => e)) + salt);

        return $"{guid:N}{hash}";
    }

    public bool ValidateKey(string key)
    {
        if (string.IsNullOrWhiteSpace(key) == true)
        {
            return false;
        }

        if (key.Length != 64) // 按算法肯定是长度 64
        {
            return false;
        }

        string guidStr = key.Substring(0, 32);
        if (Guid.TryParseExact(guidStr, "N", out Guid guid) == false)
        {
            return false;
        }

        var guidPartList = guid.ToString("D").Split('-').ToList();
        string hash = HashUtil.GetMD5(string.Concat(guidPartList.OrderByDescending(e => e)) + salt);

        return hash == key.Substring(32, 32);
    }
}