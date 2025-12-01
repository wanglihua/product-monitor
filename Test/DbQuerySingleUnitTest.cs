using Domain;
using NUnit.Framework;

namespace Test;

[TestFixture]
public class DbQuerySingleUnitTest
{
    [Test]
    public void TestQuery()
    {
        Db db = new Db();

        var user = db.User
            .Where(e => e.Id > 0)
            // .OrderBy(e => e.Id) 去掉这个会出现一个警告, 但在判断某种条件下的实体是否存在时, 这个警告并不重要
            .Take(1)
            .SingleOrDefault();
    }
}