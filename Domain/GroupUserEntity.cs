using Core;

namespace Domain;

public class GroupUserEntity : EntityNoUpdate
{
    public long GroupId { get; set; }

    public long UserId { get; set; }
}