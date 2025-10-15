using Core;

namespace Domain;

public class WebSessionEntity : Entity
{
    public string SessionId { get; set; } = "";

    public int UpdateCount { get; set; }
}