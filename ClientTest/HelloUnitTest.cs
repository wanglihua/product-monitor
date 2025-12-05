using System;
using NUnit.Framework;

namespace ClientTest
{
    [TestFixture]
    public class HelloUnitTest
    {
        [Test]
        public void SayHello()
        {
            Console.WriteLine("Hello World!");
        }
    }
}