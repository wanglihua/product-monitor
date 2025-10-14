using Core;

namespace Domain;

public class PassResEntity : EntityAssignedId
{
    public long CateId { get; set; }

    public string Name { get; set; } = "";
}