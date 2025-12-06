using Core;

namespace Domain;

/// <summary>
/// 客户端密钥
/// </summary>
public class ClientKeyEntity : Entity
{
    /// <summary>
    /// 学校
    /// </summary>
    public long CollegeId { get; set; }

    /// <summary>
    /// 产品
    /// </summary>
    public long ProductId { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public int KeyType { get; set; }

    /// <summary>
    /// 密钥
    /// </summary>
    public string ClientKey { get; set; } = ""; 

    /// <summary>
    /// 操作人
    /// </summary>
    public long UserId { get; set; }

}