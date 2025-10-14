using Core;

namespace Domain;

public class CollegeEntity : Entity
{
    public string Code { get; set; } = "";

    public string Name { get; set; } = "";

    public string Province { get; set; } = "";

    public string Level { get; set; } = "";
}