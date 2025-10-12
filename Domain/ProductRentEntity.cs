using Core;

namespace Domain;

/// <summary>
/// 租赁目录
/// </summary>
public class ProductRentEntity : Entity
{
    /// <summary>
    /// 学校 
    /// </summary>
    public long CollegeId { get; set; }

    /// <summary>
    /// 产品
    /// </summary>
    public long ProductId { get; set; }
}