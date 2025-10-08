using Core;

namespace Domain;

public class ProductUrlEntity : Entity
{
    public string Name { get; set; } = string.Empty;
    
    public string Url { get; set; } = string.Empty;
    
    public ProductUrlType Type { get; set; } = ProductUrlType.None;
    
    public string LoginUser { get; set; } = string.Empty;
    
    public string LoginPassword { get; set; } = string.Empty;
}