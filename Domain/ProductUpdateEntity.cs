using Core;

namespace Domain;

/// <summary>
/// 产品更新
/// </summary>
public class ProductUpdateEntity : Entity
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
    /// 销售版本
    /// </summary>
    public string VersionSale { get; set; } = ""; 

    /// <summary>
    /// 更新版本
    /// </summary>
    public string VersionUpdate { get; set; } = ""; 

    /// <summary>
    /// 更新次数
    /// </summary>
    public int UpdateCount { get; set; }

    /// <summary>
    /// 更新网址
    /// </summary>
    public string UpdateUrl { get; set; } = ""; 

    /// <summary>
    /// 更新账号
    /// </summary>
    public string LoginUser { get; set; } = ""; 

    /// <summary>
    /// 更新密码
    /// </summary>
    public string LoginPassword { get; set; } = ""; 

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdateDate { get; set; }

    /// <summary>
    /// 业务经理
    /// </summary>
    public string SalesManager { get; set; } = ""; 

    /// <summary>
    /// 联系老师
    /// </summary>
    public string ContactTeacher { get; set; } = ""; 

    /// <summary>
    /// 联系电话
    /// </summary>
    public string ContactMobile { get; set; } = ""; 

    /// <summary>
    /// 其他备注
    /// </summary>
    public string Description { get; set; } = ""; 

    /// <summary>
    /// 记录人
    /// </summary>
    public long UserId  { get; set; }
}