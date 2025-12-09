using Client;
using NUnit.Framework;

namespace ClientTest
{
    [TestFixture]
    public class PMUnitTest
    {
        [Test]
        public void TestStart()
        {
            // 放到 asp.net 环境下测试更好
            // 在 asp.net core 的 app.Run() 前调用，或者在 asp.net framework 的 Application_Start() 中调用 
            PM.Start(ClientType.Install, "019af8a015a67a30a9c3212aa1577b7b836dc2fcdf8d68ad8bb48ef1a416efd5");
        }
    }
}