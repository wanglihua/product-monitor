using Core;

namespace Domain;

public class UserEntity : Entity
{
    public string LoginName { get; set; } = "";

    public string UserName { get; set; } = "";

    public string Password { get; set; } = "";

    public string Salt { get; set; } = "";

    public DateTime? LastLoginTime { get; set; }
}