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

        [Test]
        public void Register()
        {
            // Try to register
            Assert.DoesNotThrow(() =>{
                Registry.Register(typeof(TestData.PlayerProfileSystem));
            });

            // Ensure that it exists
            TypeInfo playerProfileTypeInfo = Registry.GetTypeInfo(typeof(TestData.PlayerProfileSystem));
            Assert.NotNull(playerProfileTypeInfo);
        }
    }
}
