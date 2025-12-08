using Client;
using Core;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class ClientKeyService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    public string GenerateNewKey()
    {
        var guid = Guid.CreateVersion7();
        return ClientKeyUtil.GenerateKeyFromGuid(guid);
    }

    public bool ValidateKey(string key)
    {
        return ClientKeyUtil.ValidateKey(key);
    }
}