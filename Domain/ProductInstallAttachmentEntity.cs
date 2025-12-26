using Core;

namespace Domain;

/// <summary>
/// 
/// </summary>
public class ProductInstallAttachmentEntity : EntityNoUpdate
{
    /// <summary>
    /// 
    /// </summary>
    public long ProductInstallId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; } = ""; 

    /// <summary>
    /// 
    /// </summary>
    public string PathFile { get; set; } = ""; 

}