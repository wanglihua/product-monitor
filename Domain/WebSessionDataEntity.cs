using Core;

namespace Domain;

public class WebSessionDataEntity : Entity
{
    public long WebSessionDataId { get; set; }

    public WebSessionDataType Type { get; set; } = WebSessionDataType.None;

    public string Data { get; set; } = string.Empty;
}