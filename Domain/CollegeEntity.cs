using Core;

namespace Domain;

public class CollegeEntity : Entity
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Province { get; set; } = string.Empty;

    public string Level { get; set; } = string.Empty;
}