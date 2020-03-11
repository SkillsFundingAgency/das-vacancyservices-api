using Esfa.Vacancy.Domain.Constants;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Infrastructure;
using Esfa.Vacancy.Infrastructure.Ioc;
using Esfa.Vacancy.Infrastructure.Services;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using StructureMap;

namespace Esfa.Vacancy.UnitTests.Shared.Infrastructure
{
    public class GivenStructureMapRegistry
    {
        [TestCase("yes", true, TestName = "And value is 'yes' in lowercase then use sandbox service.")]
        [TestCase("YES", true, TestName = "And value is 'YES' in uppercase then use sandbox service.")]
        [TestCase("Yes", true, TestName = "And value is 'Yes' in mixed-case then use sandbox service.")]
        [TestCase("true", false, TestName = "And is any other value then use normal service.")]
        [TestCase(null, false, TestName = "And is null then use normal service.")]
        public void AndUseSandboxServiceIsEnabled(string value, bool useSandboxService)
        {
            var mockRequestContext = new Mock<IWebLoggingContext>();
            var mockProvideSettings = new Mock<IProvideSettings>();
            mockProvideSettings
                .Setup(ps => ps.GetSetting(ApplicationSettingKeys.ElasticCloudIdKey))
                .Returns("CloudKey:SomeKeyInformation");
            mockProvideSettings
                .Setup(ps => ps.GetSetting(ApplicationSettingKeys.VacancySearchUrlKey))
                .Returns("http://www.google.com");
            mockProvideSettings
                .Setup(ps => ps.GetNullableSetting(ApplicationSettingKeys.ElasticUsernameKey))
                .Returns("username");
            mockProvideSettings
                .Setup(ps => ps.GetNullableSetting(ApplicationSettingKeys.ElasticPasswordKey))
                .Returns("password");
            mockProvideSettings
                .Setup(ps => ps.GetSetting(ApplicationSettingKeys.ApprenticeshipIndexAliasKey))
                .Returns("apprenticeships");
            mockProvideSettings
                .Setup(ps => ps.GetNullableSetting(ApplicationSettingKeys.UseSandboxServices))
                .Returns(value);
            var container = new Container(new InfrastructureRegistry(mockProvideSettings.Object));

            container.Configure(c =>
            {
                c.For<IWebLoggingContext>().Use(mockRequestContext.Object);
            });

            Assert.That(container.GetInstance<ICreateApprenticeshipService>(),
                useSandboxService
                    ? Is.InstanceOf<CreateApprenticeshipSandboxService>()
                    : Is.InstanceOf<CreateApprenticeshipService>());
        }
    }
}