using Jedi;
using NUnit.Framework;

namespace Jedi.Tests
{
    public class ResgistryTests
    {
        [Test]
        public void KnownTypes()
        {
            // String
            Assert.IsNotNull(Registry.GetTypeInfo(typeof(string)));

            // DiContainer
            Assert.IsNotNull(Registry.GetTypeInfo(typeof(DiContainer)));
        }
    }
}
