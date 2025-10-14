using Core;

namespace Domain;

public class ProductEntity : Entity
{
    public string Name { get; set; } = "";

    public string Version { get; set; } = "";

    public string VersionUpdateBase { get; set; } = "";

    public string VersionUpdateDesc { get; set; } = "";

    public DateTime VersionUpdateTime { get; set; }
}