using AutoMapper;
using Esfa.Vacancy.Register.Api.DependencyResolution;
using NUnit.Framework;
using StructureMap;

namespace Esfa.Vacancy.Register.UnitTests.Shared.Api
{
    [TestFixture]
    public class AutoMapperConfigTest
    {
        [Test]
        public void ShouldHaveValidAutoMapperConfig()
        {
            var container = new Container(new DefaultRegistry());
            var config = container.GetInstance<MapperConfiguration>();

            config.AssertConfigurationIsValid();
        }
    }
}
