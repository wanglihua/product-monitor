using Core;

namespace Domain;

public class ProductUrlEntity : Entity
{
    public string Name { get; set; } = "";
    
    public string Url { get; set; } = "";
    
    public ProductUrlType Type { get; set; } = ProductUrlType.None;
    
    public string LoginUser { get; set; } = "";
    
    public string LoginPassword { get; set; } = "";
}