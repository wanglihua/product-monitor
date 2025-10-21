
using Core;

namespace Domain;

/// <summary>
/// 网址管理
/// </summary>
public class ProductUrlEntity : Entity
{
    /// <summary>
    /// 产品名称
    /// </summary>
    public string Name { get; set; } = ""; 

    /// <summary>
    /// 网址
    /// </summary>
    public string Url { get; set; } = ""; 

    /// <summary>
    /// 类型
    /// </summary>
    public ProductUrlType Type { get; set; }  = ProductUrlType.None;

    /// <summary>
    /// 账号
    /// </summary>
    public string LoginUser { get; set; } = ""; 

    /// <summary>
    /// 密码
    /// </summary>
    public string LoginPassword { get; set; } = ""; 

}