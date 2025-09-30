using Core;

namespace Domain;

public class UserEntity : Entity
{
    public string LoginName { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Salt { get; set; } = string.Empty;

    public DateTime? LastLoginTime { get; set; }
}