using Core;

namespace Domain;

public class ProductEntity : Entity
{
    public string Name { get; set; } = string.Empty;

    public string Version { get; set; } = string.Empty;

    public string VersionUpgradeBase { get; set; } = string.Empty;

    public string VersionUpgradeDesc { get; set; } = string.Empty;

    public DateTime VersionUpgradeTime { get; set; }
}