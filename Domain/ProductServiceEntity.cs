using Core;

namespace Domain;

/// <summary>
/// 售后服务
/// </summary>
public class ProductServiceEntity : Entity
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
    /// 项目类型
    /// </summary>
    public ProjectType ProjectType { get; set; } = ProjectType.None;

    /// <summary>
    /// 售后时间
    /// </summary>
    public DateTime ServiceDate { get; set; }

    /// <summary>
    /// 售后次数
    /// </summary>
    public int ServiceCount { get; set; }

    /// <summary>
    /// 售后网址
    /// </summary>
    public string ServiceUrl { get; set; } = "";

    /// <summary>
    /// 售后账号
    /// </summary>
    public string LoginUser { get; set; } = "";

    /// <summary>
    /// 售后密码
    /// </summary>
    public string LoginPassword { get; set; } = "";

    /// <summary>
    /// 联系老师
    /// </summary>
    public string ContactTeacher { get; set; } = "";

    /// <summary>
    /// 联系电话
    /// </summary>
    public string ContactMobile { get; set; } = "";

    /// <summary>
    /// 售后问题
    /// </summary>
    public string Description { get; set; } = "";

    /// <summary>
    /// 记录人
    /// </summary>
    public long UserId { get; set; }
}