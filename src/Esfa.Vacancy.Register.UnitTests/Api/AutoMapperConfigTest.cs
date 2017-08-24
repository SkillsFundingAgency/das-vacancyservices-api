using AutoMapper;
using Esfa.Vacancy.Register.Api.App_Start;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.Api
{
    [TestFixture]
    public class AutoMapperConfigTest
    {
        [Test]
        public void ShouldHaveValidAutoMapperConfig()
        {
            AutoMapperConfig.Configure();

            Mapper.Configuration.AssertConfigurationIsValid();
        }
        
    }
}
