using Core.CodeType;

namespace Domain;

public class ActionLogType : CodeTypeBase
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
    };

    private static readonly IDictionary<int, ActionLogType> codeTypeMap = codeTypeList.ToDictionary(e => e.Code);

    public static ActionLogType GetByCode(int code)
    {
        if (codeTypeMap.TryGetValue(code, out var codeType))
        {
            return codeType;
        }

        return None;
    }

    public static IList<ActionLogType> GetList()
    {
        return codeTypeList.ToList();
    }

    public ActionLogType(int code, string name) : base(code, name)
    {
    }
}

public class ActionLogTypeModelBinder : CodeTypeModelBinder<ActionLogType>
{
}

public class ActionLogTypeCodeJsonConverter : CodeTypeJsonConverter<ActionLogType>
{
}

public class ActionLogTypeValueConverter : CodeTypeValueConverter<ActionLogType>
{
}