using Core.CodeType;

namespace Domain;

public class ActionLogType : CodeTypeBase, ICodeType<ActionLogType>
{
    /// <summary>
    /// 用户登录
    /// </summary>
    public static readonly ActionLogType UserLogin = new(0002_0000, "用户登录");

    /// <summary>
    /// 用户创建
    /// </summary>
    public static readonly ActionLogType UserCreate = new(0004_0000, "用户创建");

    /// <summary>
    /// 用户更新
    /// </summary>
    public static readonly ActionLogType UserUpdate = new(0006_0000, "用户更新");

    /// <summary>
    /// 用户删除
    /// </summary>
    public static readonly ActionLogType UserDelete = new(0008_0000, "用户删除");

    /// <summary>
    /// 管理员添加
    /// </summary>
    public static readonly ActionLogType AdminUserAdd = new(0010_0000, "管理员添加");

    /// <summary>
    /// 管理员移除
    /// </summary>
    public static readonly ActionLogType AdminUserRemove = new(0012_0000, "管理员移除");

    /// <summary>
    /// 权限资源数据更新
    /// </summary>
    public static readonly ActionLogType PassResDataInit = new(0014_0000, "权限资源数据更新");

    /// <summary>
    /// 产品目录创建
    /// </summary>
    public static readonly ActionLogType ProductCreate = new(0016_0000, "产品目录创建");

    /// <summary>
    /// 产品目录更新
    /// </summary>
    public static readonly ActionLogType ProductUpdate = new(0018_0000, "产品目录更新");

    /// <summary>
    /// 产品目录删除
    /// </summary>
    public static readonly ActionLogType ProductDelete = new(0020_0000, "产品目录删除");

    /// <summary>
    /// 院校目录创建
    /// </summary>
    public static readonly ActionLogType CollegeCreate = new(0022_0000, "院校目录创建");

    /// <summary>
    /// 院校目录更新
    /// </summary>
    public static readonly ActionLogType CollegeUpdate = new(0024_0000, "院校目录更新");

    /// <summary>
    /// 院校目录删除
    /// </summary>
    public static readonly ActionLogType CollegeDelete = new(0026_0000, "院校目录删除");

    /// <summary>
    /// 网址管理创建
    /// </summary>
    public static readonly ActionLogType ProductUrlCreate = new(0028_0000, "网址管理创建");

    /// <summary>
    /// 网址管理更新
    /// </summary>
    public static readonly ActionLogType ProductUrlUpdate = new(0030_0000, "网址管理更新");

    /// <summary>
    /// 网址管理删除
    /// </summary>
    public static readonly ActionLogType ProductUrlDelete = new(0032_0000, "网址管理删除");

    /// <summary>
    /// 销售目录创建
    /// </summary>
    public static readonly ActionLogType ProductSaleCreate = new(0034_0000, "销售目录创建");

    /// <summary>
    /// 销售目录更新
    /// </summary>
    public static readonly ActionLogType ProductSaleUpdate = new(0036_0000, "销售目录更新");

    /// <summary>
    /// 销售目录删除
    /// </summary>
    public static readonly ActionLogType ProductSaleDelete = new(0038_0000, "销售目录删除");

    /// <summary>
    /// 销售目录明细创建
    /// </summary>
    public static readonly ActionLogType ProductSaleEntryCreate = new(0040_0000, "销售目录明细创建");

    /// <summary>
    /// 销售目录明细更新
    /// </summary>
    public static readonly ActionLogType ProductSaleEntryUpdate = new(0042_0000, "销售目录明细更新");

    /// <summary>
    /// 销售目录明细删除
    /// </summary>
    public static readonly ActionLogType ProductSaleEntryDelete = new(0044_0000, "销售目录明细删除");

    /// <summary>
    /// 租赁目录创建
    /// </summary>
    public static readonly ActionLogType ProductRentCreate = new(0046_0000, "租赁目录创建");

    /// <summary>
    /// 租赁目录更新
    /// </summary>
    public static readonly ActionLogType ProductRentUpdate = new(0048_0000, "租赁目录更新");

    /// <summary>
    /// 租赁目录删除
    /// </summary>
    public static readonly ActionLogType ProductRentDelete = new(0050_0000, "租赁目录删除");

    /// <summary>
    /// 租赁目录明细创建
    /// </summary>
    public static readonly ActionLogType ProductRentEntryCreate = new(0052_0000, "租赁目录明细创建");

    /// <summary>
    /// 租赁目录明细更新
    /// </summary>
    public static readonly ActionLogType ProductRentEntryUpdate = new(0054_0000, "租赁目录明细更新");

    /// <summary>
    /// 租赁目录明细删除
    /// </summary>
    public static readonly ActionLogType ProductRentEntryDelete = new(0056_0000, "租赁目录明细删除");


    /// <summary>
    /// 试用目录创建
    /// </summary>
    public static readonly ActionLogType ProductEvalCreate = new(0058_0000, "试用目录创建");

    /// <summary>
    /// 试用目录更新
    /// </summary>
    public static readonly ActionLogType ProductEvalUpdate = new(0060_0000, "试用目录更新");

    /// <summary>
    /// 试用目录删除
    /// </summary>
    public static readonly ActionLogType ProductEvalDelete = new(0062_0000, "试用目录删除");

    /// <summary>
    /// 试用目录明细创建
    /// </summary>
    public static readonly ActionLogType ProductEvalEntryCreate = new(0064_0000, "试用目录明细创建");

    /// <summary>
    /// 试用目录明细更新
    /// </summary>
    public static readonly ActionLogType ProductEvalEntryUpdate = new(0066_0000, "试用目录明细更新");

    /// <summary>
    /// 试用目录明细删除
    /// </summary>
    public static readonly ActionLogType ProductEvalEntryDelete = new(0068_0000, "试用目录明细删除");

    /// <summary>
    /// 产品安装创建
    /// </summary>
    public static readonly ActionLogType ProductInstallCreate = new(0070_0000, "产品安装创建");

    /// <summary>
    /// 产品安装更新
    /// </summary>
    public static readonly ActionLogType ProductInstallUpdate = new(0072_0000, "产品安装更新");

    /// <summary>
    /// 产品安装删除
    /// </summary>
    public static readonly ActionLogType ProductInstallDelete = new(0074_0000, "产品安装删除");

    /// <summary>
    /// 产品更新创建
    /// </summary>
    public static readonly ActionLogType ProductUpdateCreate = new(0076_0000, "产品更新创建");

    /// <summary>
    /// 产品更新更新
    /// </summary>
    public static readonly ActionLogType ProductUpdateUpdate = new(0078_0000, "产品更新更新");

    /// <summary>
    /// 产品更新删除
    /// </summary>
    public static readonly ActionLogType ProductUpdateDelete = new(0080_0000, "产品更新删除");

    /// <summary>
    /// 售后服务创建
    /// </summary>
    public static readonly ActionLogType ProductServiceCreate = new(0082_0000, "售后服务创建");

    /// <summary>
    /// 售后服务更新
    /// </summary>
    public static readonly ActionLogType ProductServiceUpdate = new(0084_0000, "售后服务更新");

    /// <summary>
    /// 售后服务删除
    /// </summary>
    public static readonly ActionLogType ProductServiceDelete = new(0086_0000, "售后服务删除");

    /// <summary>
    /// 特殊售后创建
    /// </summary>
    public static readonly ActionLogType ProductSpecialServiceCreate = new(0088_0000, "特殊售后创建");

    /// <summary>
    /// 特殊售后更新
    /// </summary>
    public static readonly ActionLogType ProductSpecialServiceUpdate = new(0090_0000, "特殊售后更新");

    /// <summary>
    /// 特殊售后删除
    /// </summary>
    public static readonly ActionLogType ProductSpecialServiceDelete = new(0092_0000, "特殊售后删除");

    /// <summary>
    /// 产品版本创建
    /// </summary>
    public static readonly ActionLogType ProductVersionCreate = new(0094_0000, "产品版本创建");

    /// <summary>
    /// 产品版本更新
    /// </summary>
    public static readonly ActionLogType ProductVersionUpdate = new(0096_0000, "产品版本更新");

    /// <summary>
    /// 产品版本删除
    /// </summary>
    public static readonly ActionLogType ProductVersionDelete = new(0098_0000, "产品版本删除");

    /// <summary>
    /// 安装监测创建
    /// </summary>
    public static readonly ActionLogType ProductInstallMonitorCreate = new(0100_0000, "安装监测创建");

    /// <summary>
    /// 安装监测更新
    /// </summary>
    public static readonly ActionLogType ProductInstallMonitorUpdate = new(0102_0000, "安装监测更新");

    /// <summary>
    /// 安装监测删除
    /// </summary>
    public static readonly ActionLogType ProductInstallMonitorDelete = new(0104_0000, "安装监测删除");

    /// <summary>
    /// 院校使用详情创建
    /// </summary>
    public static readonly ActionLogType ProductInstallMonitorEntryCreate = new(0106_0000, "院校使用详情创建");

    /// <summary>
    /// 院校使用详情更新
    /// </summary>
    public static readonly ActionLogType ProductInstallMonitorEntryUpdate = new(0108_0000, "院校使用详情更新");

    /// <summary>
    /// 院校使用详情删除
    /// </summary>
    public static readonly ActionLogType ProductInstallMonitorEntryDelete = new(0110_0000, "院校使用详情删除");

    /// <summary>
    /// 网站监测创建
    /// </summary>
    public static readonly ActionLogType ProductUrlMonitorCreate = new(0112_0000, "网站监测创建");

    /// <summary>
    /// 网站监测更新
    /// </summary>
    public static readonly ActionLogType ProductUrlMonitorUpdate = new(0114_0000, "网站监测更新");

    /// <summary>
    /// 网站监测删除
    /// </summary>
    public static readonly ActionLogType ProductUrlMonitorDelete = new(0116_0000, "网站监测删除");

    /// <summary>
    /// 网站监测详情创建
    /// </summary>
    public static readonly ActionLogType ProductUrlMonitorEntryCreate = new(0118_0000, "网站监测详情创建");

    /// <summary>
    /// 网站监测详情更新
    /// </summary>
    public static readonly ActionLogType ProductUrlMonitorEntryUpdate = new(0120_0000, "网站监测详情更新");

    /// <summary>
    /// 网站监测详情删除
    /// </summary>
    public static readonly ActionLogType ProductUrlMonitorEntryDelete = new(0122_0000, "网站监测详情删除");

    /// <summary>
    /// 异常检测创建
    /// </summary>
    public static readonly ActionLogType ExceptionMonitorCreate = new(0124_0000, "异常检测创建");

    /// <summary>
    /// 异常检测更新
    /// </summary>
    public static readonly ActionLogType ExceptionMonitorUpdate = new(0126_0000, "异常检测更新");

    /// <summary>
    /// 异常检测删除
    /// </summary>
    public static readonly ActionLogType ExceptionMonitorDelete = new(0128_0000, "异常检测删除");

    /// <summary>
    /// None
    /// </summary>
    public static readonly ActionLogType None = new(0, "");

    private static readonly IReadOnlyList<ActionLogType> codeTypeList = new List<ActionLogType>() {
        UserLogin,
        UserCreate,
        UserUpdate,
        UserDelete,
        AdminUserAdd,
        AdminUserRemove,
        PassResDataInit,
        ProductCreate,
        ProductUpdate,
        ProductDelete,
        CollegeCreate,
        CollegeUpdate,
        CollegeDelete,
        ProductUrlCreate,
        ProductUrlUpdate,
        ProductUrlDelete,
        ProductSaleCreate,
        ProductSaleUpdate,
        ProductSaleDelete,
        ProductSaleEntryCreate,
        ProductSaleEntryUpdate,
        ProductSaleEntryDelete,
        ProductRentCreate,
        ProductRentUpdate,
        ProductRentDelete,
        ProductRentEntryCreate,
        ProductRentEntryUpdate,
        ProductRentEntryDelete,
        ProductEvalCreate,
        ProductEvalUpdate,
        ProductEvalDelete,
        ProductEvalEntryCreate,
        ProductEvalEntryUpdate,
        ProductEvalEntryDelete,
        ProductInstallCreate,
        ProductInstallUpdate,
        ProductInstallDelete,
        ProductUpdateCreate,
        ProductUpdateUpdate,
        ProductUpdateDelete,
        ProductServiceCreate,
        ProductServiceUpdate,
        ProductServiceDelete,
        ProductSpecialServiceCreate,
        ProductSpecialServiceUpdate,
        ProductSpecialServiceDelete,
        ProductVersionCreate,
        ProductVersionUpdate,
        ProductVersionDelete,
        ProductInstallMonitorCreate,
        ProductInstallMonitorUpdate,
        ProductInstallMonitorDelete,
        ProductInstallMonitorEntryCreate,
        ProductInstallMonitorEntryUpdate,
        ProductInstallMonitorEntryDelete,
        ProductUrlMonitorCreate,
        ProductUrlMonitorUpdate,
        ProductUrlMonitorDelete,
        ProductUrlMonitorEntryCreate,
        ProductUrlMonitorEntryUpdate,
        ProductUrlMonitorEntryDelete,
        ExceptionMonitorCreate,
        ExceptionMonitorUpdate,
        ExceptionMonitorDelete,
    };

    private static readonly IDictionary<int, ActionLogType> codeTypeMap = codeTypeList.ToDictionary(e => e.Id);

    public static ActionLogType GetById(int id)
    {
        if (codeTypeMap.TryGetValue(id, out var codeType))
        {
            return codeType;
        }

        return None;
    }

    public static IList<ActionLogType> GetList()
    {
        return codeTypeList.ToList();
    }

    private ActionLogType(int id, string name) : base(id, name)
    {
    }
}