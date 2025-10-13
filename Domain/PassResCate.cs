namespace Domain;

public static class PassResCate
{
    // 数据监测
    public const long DataMonitorId = 0200_0000;
    public static readonly PassResCateEntity DataMonitor = new() { Id = DataMonitorId, Name = "数据监测" };

    // 产品服务
    public const long ProductServiceId = 0400_0000;
    public static readonly PassResCateEntity ProductService = new() { Id = ProductServiceId, Name = "产品服务" };

    // 项目管理
    public const long ProjectManageId = 0600_0000;
    public static readonly PassResCateEntity ProjectManage = new() { Id = ProjectManageId, Name = "项目管理" };

    // 基础设置
    public const long BasicSettingId = 0800_0000;
    public static readonly PassResCateEntity BasicSetting = new() { Id = BasicSettingId, Name = "基础设置" };

    // 系统管理
    public const long SystemManageId = 1000_0000;
    public static readonly PassResCateEntity SystemManage = new() { Id = SystemManageId, Name = "系统管理" };

    public static IList<PassResCateEntity> GetLst()
    {
        return new List<PassResCateEntity>() {
            DataMonitor,
            ProductService,
            ProjectManage,
            BasicSetting,
            SystemManage,
        };
    }
}