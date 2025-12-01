using Domain;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Test;

[TestFixture]
public class DbJointTrackingUnitTest
{
    [Test]
    public void TestTracking()
    {
        // TODO 测试 Join 时的 Tacking 特性

        Db db = new Db();

        var adminUser = db.AdminUser
            .Join(db.User,
                admin => admin.UserId,
                user => user.Id,
                (admin, user) => new { Admin = admin, User = user }
            )
            .AsNoTracking()
            .Where(e => e.User.Id == 1)
            .Take(1)
            .SingleOrDefault();

        if (adminUser != null)
        {
            // db.User.Update(adminUser.User);

            adminUser.User.UserName = "管理员123";

            db.SaveChanges();
        }
    }
}