using NUnit.Framework;

namespace Tests
{
    public class HelloTest
    {
        [Test]
        public void hello_test()
        {
            Assert.AreEqual("Hello Test!", "Hello Test!");
        }
    }
}