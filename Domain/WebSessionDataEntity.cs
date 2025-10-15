using Core;

namespace Domain;

public class WebSessionDataEntity : Entity
{
    public long WebSessionId { get; set; }

    public string SessionId { get; set; } = "";

    public WebSessionDataType Type { get; set; } = WebSessionDataType.None;

    public string Data { get; set; } = "";
}