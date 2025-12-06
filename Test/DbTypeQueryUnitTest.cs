using Domain;
using NUnit.Framework;

namespace Test;

[TestFixture]
public class DbTypeQueryUnitTest
{
    [Test]
    public void TestDbTypeQuery()
    {
        Db db = new Db();

        int serviceTimes = db.ProductService.Count(e => e.ProjectType == ProjectType.Sale);
        Console.WriteLine(serviceTimes);
    }
}