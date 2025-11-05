using Core;

namespace Domain;

/// <summary>
/// 网站监测
/// </summary>
public class ProductUrlMonitorEntity : Entity
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
    /// 分类
    /// </summary>
    public ProductUrlType ProductUrlType { get; set; } = ProductUrlType.None;

    /// <summary>
    /// 任务数量
    /// </summary>
    public int TaskCount { get; set; }

    /// <summary>
    /// 教师数量
    /// </summary>
    public int TeacherCount { get; set; }
}