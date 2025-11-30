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
            .OrderBy(e => e.Id)
            .Take(1)
            .SingleOrDefault();
    }
}