using Jedi;
using NUnit.Framework;

namespace Jedi.Tests
{
    public class ResgistryTests
    {
        [Test]
        public void PrimitiveTypes()
        {
            Assert.IsNotNull(Registry.GetTypeInfo(typeof(bool)));
            Assert.IsNotNull(Registry.GetTypeInfo(typeof(byte)));
            Assert.IsNotNull(Registry.GetTypeInfo(typeof(sbyte)));
            Assert.IsNotNull(Registry.GetTypeInfo(typeof(char)));
            Assert.IsNotNull(Registry.GetTypeInfo(typeof(decimal)));
            Assert.IsNotNull(Registry.GetTypeInfo(typeof(double)));
            Assert.IsNotNull(Registry.GetTypeInfo(typeof(float)));
            Assert.IsNotNull(Registry.GetTypeInfo(typeof(int)));
            Assert.IsNotNull(Registry.GetTypeInfo(typeof(uint)));
            Assert.IsNotNull(Registry.GetTypeInfo(typeof(long)));
            Assert.IsNotNull(Registry.GetTypeInfo(typeof(ulong)));
            Assert.IsNotNull(Registry.GetTypeInfo(typeof(short)));
            Assert.IsNotNull(Registry.GetTypeInfo(typeof(ushort)));
            Assert.IsNotNull(Registry.GetTypeInfo(typeof(string)));
        }
    }
}
