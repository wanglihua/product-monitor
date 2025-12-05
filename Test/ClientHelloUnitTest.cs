using Client;
using NUnit.Framework;

namespace Test;

[TestFixture]
public class ClientHelloUnitTest
{
    [Test]
    public void TestSayHello()
    {
        var hello = new Hello();
        hello.SayHello();
    }
}