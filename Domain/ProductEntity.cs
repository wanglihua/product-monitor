using Core;

namespace Domain;

public class ProductEntity : Entity
{
    public string Name { get; set; } = string.Empty;

    public string Version { get; set; } = string.Empty;

    public string VersionUpdateBase { get; set; } = string.Empty;

    public string VersionUpdateDesc { get; set; } = string.Empty;

    public DateTime VersionUpdateTime { get; set; }
}