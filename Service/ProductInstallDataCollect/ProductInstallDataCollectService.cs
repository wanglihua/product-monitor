using Core;

namespace Service.ProductInstallDataCollect;

[Service(ServiceType.Scoped)]
public class ProductInstallDataCollectService
{
    
    
    public void CheckParam(ProductInstallDataCollectParam param)
    {
        string key = param.Key;
        string useTime = param.UseTime;
    }
}