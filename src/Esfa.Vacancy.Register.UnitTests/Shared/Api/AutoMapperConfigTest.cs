using Esfa.Vacancy.Register.Api;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.Shared.Api
{
    [TestFixture]
    public class AutoMapperConfigTest
    {
        [Test]
        public void ShouldHaveValidAutoMapperConfig()
        {
            AutoMapperConfig.Configure().AssertConfigurationIsValid();
        }
    }
}
