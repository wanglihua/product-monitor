using Client;
using Core;

namespace Service.ProductUrlDataCollect;

[Service(ServiceType.Scoped)]
public class ProductUrlDataCollectService
{
    public bool VerifySign(ProductUrlDataCollectParam param)
    {
        return DataCollectUtil.VerifySign(param);
    }
}
