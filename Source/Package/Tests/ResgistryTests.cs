using Jedi;
using NUnit.Framework;
using System.Linq;

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

            // Ensure that the Ctr, Fields, Properties and Methods are set.
            // 1. Ctr
            Assert.NotNull(playerProfileTypeInfo.Ctr);
            Assert.AreEqual(
                playerProfileTypeInfo.Ctr.Info, 
                typeof(TestData.PlayerProfileSystem).
                    GetConstructors(TypeInfo.BindingFlag).
                    FirstOrDefault()
            );

            // 2. Fields
            Assert.NotNull(playerProfileTypeInfo.Fields);

            // 3. Properties
            Assert.NotNull(playerProfileTypeInfo.Properties);

            // 4. Methods
            Assert.NotNull(playerProfileTypeInfo.Methods);
        }
    }
}
