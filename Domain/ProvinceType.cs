using Core.CodeType;

namespace Domain;

/// <summary>
/// 省份
/// </summary>
public class ProvinceType : CodeTypeBase, ICodeType<ProvinceType>
{
    /// <summary>
    /// 北京市
    /// </summary>
    public static readonly ProvinceType BeiJing = new(1_00, "北京市");

    /// <summary>
    /// 天津市
    /// </summary>
    public static readonly ProvinceType TianJing = new(2_00, "天津市");

    /// <summary>
    /// 上海市
    /// </summary>
    public static readonly ProvinceType ShangHai = new(3_00, "上海市");

    /// <summary>
    /// 重庆市
    /// </summary>
    public static readonly ProvinceType ChongQing = new(4_00, "重庆市");

    /// <summary>
    /// 河北省
    /// </summary>
    public static readonly ProvinceType HeBei = new(5_00, "河北省");

    /// <summary>
    /// 山西省
    /// </summary>
    public static readonly ProvinceType ShanXin = new(6_00, "山西省"); // 跟 陕西省 重名了

    /// <summary>
    /// 辽宁省
    /// </summary>
    public static readonly ProvinceType LiaoNing = new(7_00, "辽宁省");

    /// <summary>
    /// 吉林省
    /// </summary>
    public static readonly ProvinceType JiLing = new(8_00, "吉林省");

    /// <summary>
    /// 黑龙江省
    /// </summary>
    public static readonly ProvinceType HeiLongJiang = new(9_00, "黑龙江省");

    /// <summary>
    /// 江苏省
    /// </summary>
    public static readonly ProvinceType JiangSu = new(10_00, "江苏省");

    /// <summary>
    /// 浙江省
    /// </summary>
    public static readonly ProvinceType ZheJiang = new(11_00, "浙江省");

    /// <summary>
    /// 安徽省
    /// </summary>
    public static readonly ProvinceType AnHui = new(12_00, "安徽省");

    /// <summary>
    /// 福建省
    /// </summary>
    public static readonly ProvinceType FuJian = new(13_00, "福建省");

    /// <summary>
    /// 江西省
    /// </summary>
    public static readonly ProvinceType JiangXi = new(14_00, "江西省");

    /// <summary>
    /// 山东省
    /// </summary>
    public static readonly ProvinceType ShanDong = new(15_00, "山东省");

    /// <summary>
    /// 河南省
    /// </summary>
    public static readonly ProvinceType HeNan = new(16_00, "河南省");

    /// <summary>
    /// 湖北省
    /// </summary>
    public static readonly ProvinceType HuBei = new(17_00, "湖北省");

    /// <summary>
    /// 湖南省
    /// </summary>
    public static readonly ProvinceType HuNan = new(18_00, "湖南省");

    /// <summary>
    /// 广东省
    /// </summary>
    public static readonly ProvinceType GuangDong = new(19_00, "广东省");

    /// <summary>
    /// 海南省
    /// </summary>
    public static readonly ProvinceType HaiNan = new(20_00, "海南省");

    /// <summary>
    /// 四川省
    /// </summary>
    public static readonly ProvinceType SiChuan = new(21_00, "四川省");

    /// <summary>
    /// 贵州省
    /// </summary>
    public static readonly ProvinceType GuiZhou = new(22_00, "贵州省");

    /// <summary>
    /// 云南省
    /// </summary>
    public static readonly ProvinceType YuNan = new(23_00, "云南省");

    /// <summary>
    /// 陕西省
    /// </summary>
    public static readonly ProvinceType ShanXi = new(24_00, "陕西省"); // 跟 山西省 重名了

    /// <summary>
    /// 甘肃省
    /// </summary>
    public static readonly ProvinceType GanSu = new(25_00, "甘肃省");

    /// <summary>
    /// 青海省
    /// </summary>
    public static readonly ProvinceType QingHai = new(26_00, "青海省");

    /// <summary>
    /// 台湾省
    /// </summary>
    public static readonly ProvinceType TaiWan = new(27_00, "台湾省");

    /// <summary>
    /// 内蒙古自治区
    /// </summary>
    public static readonly ProvinceType NeiMengGu = new(28_00, "内蒙古自治区");

    /// <summary>
    /// 广西壮族自治区
    /// </summary>
    public static readonly ProvinceType GuangXi = new(29_00, "广西壮族自治区");

    /// <summary>
    /// 西藏自治区
    /// </summary>
    public static readonly ProvinceType XiZang = new(30_00, "西藏自治区");

    /// <summary>
    /// 宁夏回族自治区
    /// </summary>
    public static readonly ProvinceType NingXia = new(31_00, "宁夏回族自治区");

    /// <summary>
    /// 新疆维吾尔自治区
    /// </summary>
    public static readonly ProvinceType XinJiang = new(32_00, "新疆维吾尔自治区");

    /// <summary>
    /// 香港特别行政区
    /// </summary>
    public static readonly ProvinceType HongKong = new(33_00, "香港特别行政区");

    /// <summary>
    /// 澳门特别行政区
    /// </summary>
    public static readonly ProvinceType Macao = new(34_00, "澳门特别行政区");

    /// <summary>
    /// None
    /// </summary>
    public static readonly ProvinceType None = new(0, "");

    private static readonly IReadOnlyList<ProvinceType> codeTypeList = new List<ProvinceType>() {
        BeiJing,
        TianJing,
        ShangHai,
        ChongQing,
        HeBei,
        ShanXin, // 跟 陕西省 重名了
        LiaoNing,
        JiLing,
        HeiLongJiang,
        JiangSu,
        ZheJiang,
        AnHui,
        FuJian,
        JiangXi,
        ShanDong,
        HeNan,
        HuBei,
        HuNan,
        GuangDong,
        HaiNan,
        SiChuan,
        GuiZhou,
        YuNan,
        ShanXi, // 跟 山西省 重名了
        GanSu,
        QingHai,
        TaiWan,
        NeiMengGu,
        GuangXi,
        XiZang,
        NingXia,
        XinJiang,
        HongKong,
        Macao,
    };

    public static IList<ProvinceType> GetList()
    {
        return codeTypeList.ToList();
    }

    private static readonly IDictionary<int, ProvinceType> codeTypeMap = codeTypeList.ToDictionary(e => e.Id);

    public static ProvinceType GetById(int id)
    {
        if (codeTypeMap.TryGetValue(id, out var codeType))
        {
            return codeType;
        }

        return None;
    }

    private ProvinceType(int id, string name) : base(id, name)
    {
    }
}
