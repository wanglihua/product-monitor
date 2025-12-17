using Core;
using Domain;
// using Microsoft.EntityFrameworkCore;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class UserProvinceService // (Db db)
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    public IList<ProvinceType> GetUserProvinceList(long userId)
    {
        return ProvinceType.GetList();

        // var userProvinceList = db.UserProvince.AsNoTracking()
        //     .Where(e => e.UserId == userId)
        //     .ToList();
        //
        // var provinceTypeList = userProvinceList.Select(e => e.ProvinceType).ToList();
        //
        // // 这是防止返回的列表是空的， 造成SQL查询时错误
        // if (provinceTypeList.Count == 0)
        // {
        //     provinceTypeList.Add(ProvinceType.None);
        // }
        //
        // return provinceTypeList;
    }
}