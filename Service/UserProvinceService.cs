using Core;
using Domain;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class UserProvinceService(Db db)
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    public IList<ProvinceType> GetUserProvinceList(long userId)
    {
        var userProvinceList = db.UserProvince.AsNoTracking()
            .Where(e => e.UserId == userId)
            .ToList();

        return userProvinceList.Select(e => e.ProvinceType).ToList();
    }
}