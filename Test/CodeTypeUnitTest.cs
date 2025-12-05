using Domain;
using NUnit.Framework;

namespace Test;

[TestFixture]
public class CodeTypeUnitTest
{
    private enum DummyType
    {
        Member1 = 1,
        Member2 = 2,
        Member3 = 2 // 两个成员的值相同的时候，他也不会报错
    }

    [Test]
    public void TestCodeTypeEqual()
    {
        var status1 = UserStatus.Enabled;
        // var status2 = new UserStatus(100, "已启用");
        var status2 = UserStatus.Enabled;

        Assert.That(status1 == status2);

        status1 = UserStatus.Enabled;
        // status2 = new UserStatus(100, "已启用, 已启用");
        status2 = UserStatus.Disabled;

        Assert.That(status1 != status2);
    }
}