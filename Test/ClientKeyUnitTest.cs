using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using NUnit.Framework;
using Service;

namespace Test;

[TestFixture]
public class ClientKeyUnitTest
{
    [Test]
    public void TestGenerateKey()
    {
        var clientKeyService = new ClientKeyService();
        string key = clientKeyService.GenerateNewKey();
        Console.WriteLine(key);

        bool valid = clientKeyService.ValidateKey(key);
        Assert.That(valid, Is.True);
    }
}