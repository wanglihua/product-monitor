using Core;
using Domain;

namespace Service;

[Service(ServiceType.Scoped)]
public class AdminUserService(Db db)
{
    public bool IsAdminUser(long userId)
    {
        if (userId <= 0)
        {
            return false;
        }

        var adminUserExist = db.AdminUser
            .Where(e => e.UserId == userId)
            .OrderBy(e => e.UserId)
            .Take(1)
            .SingleOrDefault();

        if (adminUserExist is not null)
        {
            return true;
        }

        return false;
    }
}