using Core;

namespace Domain;

public class UserPassResEntity : EntityNoUpdate
{
    public long UserId { get; set; }

    public long PassResId { get; set; }
}