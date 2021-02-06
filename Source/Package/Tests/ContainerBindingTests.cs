using FluentAssertions;
using NUnit.Framework;

namespace Jedi.Tests
{
    public class ContainerBindingTests
    {
        private readonly DiContainer _container;

        public ContainerBindingTests()
        {
            _container = new DiContainer();
        }

        [Test]
        public void Bind_BindType_CreatesContract()
        {
            var result = _container.Bind(typeof(TestData.Database));

            _container.GetContract(typeof(TestData.Database)).Should().Be(result);
        }

        [Test]
        public void Bind_BindTypeWithId_CreatesContractWithId()
        {
            var id = "anyId";
            
            var result = _container.Bind(typeof(TestData.Database), id);

            _container.GetContract(id).Should().Be(result);   
        }
        
        [Test]
        public void Bind_BindTypeGeneric_CreatesContract()
        {
            var container = new DiContainer();

            var result = container.Bind<TestData.Database>();

            container.GetContract(typeof(TestData.Database)).Should().Be(result);
        }
    }
}