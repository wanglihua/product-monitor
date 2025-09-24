using Core;

namespace Service;

[Service(ServiceType.Scoped)]
public class WebSessionTimeoutService
{
    public void ResetTimeout(string sessionId)
    {
    }
}