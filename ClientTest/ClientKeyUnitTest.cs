using Client;
using NUnit.Framework;

namespace ClientTest
{
    [TestFixture]
    public class ClientKeyUnitTest
    {
        [Test]
        public void TestValidClientKey()
        {
            // 019af8a015a67a30a9c3212aa1577b7b836dc2fcdf8d68ad8bb48ef1a416efd5
            var clientKeyService = new ClientKeyService();
            bool valid = clientKeyService.ValidateKey("019af8a015a67a30a9c3212aa1577b7b836dc2fcdf8d68ad8bb48ef1a416efd5");
            Assert.That(valid, Is.True);
        }
    }
}