using Client;
using Core;

namespace Service.ProductInstallDataCollect;

[Service(ServiceType.Scoped)]
public class ProductInstallDataCollectService
{
    public bool VerifySign(ProductInstallDataCollectParam param)
    {
        return DataCollectUtil.VerifySign(param);
    }
}