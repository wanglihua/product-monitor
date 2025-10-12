namespace Domain.PassRes;

public class PassResCate
{
    // 数据监测
    public const int DataMonitorId = 0200_0000;
    public static readonly PassResCateEntity DataMonitorEntity = new() { Id = DataMonitorId, Name = "数据监测" };

    // 产品服务
    public const int ProductServiceId = 0400_0000;
    public static readonly PassResCateEntity ProductService = new() { Id = ProductServiceId, Name = "产品服务" };

    // 项目管理
    public const int ProjectManageId = 0600_0000;
    public static readonly PassResCateEntity ProjectManage = new() { Id = ProjectManageId, Name = "项目管理" };

    // 基础设置
    public const int BasicSettingId = 0800_0000;
    public static readonly PassResCateEntity BasicSetting = new() { Id = BasicSettingId, Name = "基础设置" };

    // 操作日志
    public const int ActionLogId = 1000_0000;
    public static readonly PassResCateEntity ActionLog = new() { Id = ActionLogId, Name = "操作日志" };
}