using Domain;
using Microsoft.Identity.Client;
using NUnit.Framework;

namespace Test;

[TestFixture]
public class DbUpdateSameDataUnitTest
{
    [Test]
    public void TestUpdateSameData()
    {
        Db db = new Db();

        var user = db.User.Find(1L);

        if (user is not null)
        {
            user.LoginName = "admin";

            // 加了这个他会肯定生成 update 语句， 如果不加设置的字段跟数据库值一样的时候， 不会实际更新
            // db.User.Update(user); 

            db.SaveChanges();
        }
    }
}