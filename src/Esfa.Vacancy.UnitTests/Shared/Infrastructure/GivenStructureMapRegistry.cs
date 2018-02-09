using Esfa.Vacancy.Domain.Constants;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Infrastructure;
using Esfa.Vacancy.Infrastructure.Services;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using StructureMap;

namespace Esfa.Vacancy.UnitTests.Shared.Infrastructure
{
    public class GivenStructureMapRegistry
    {
        [Test]
        public void AndUseSandboxServiceIsEnabled()
        {
            var mockRequestContext = new Mock<IRequestContext>();
            var mockProvideSettings = new Mock<IProvideSettings>();
            mockProvideSettings
                .Setup(ps => ps.GetNullableSetting(ApplicationSettingKeys.UseSandboxServices))
                .Returns("something");
            var container = new Container(new InfrastructureRegistry(mockProvideSettings.Object));

            container.Configure(c =>
            {
                c.For<IRequestContext>().Use(mockRequestContext.Object);
            });

            Assert.That(container.GetInstance<IProvideSettings>(),
                Is.SameAs(mockProvideSettings.Object));

            Assert.That(container.GetInstance<ICreateApprenticeshipService>(),
                Is.InstanceOf<CreateApprenticeshipSandboxService>());
        }

        [Test]
        public void AndUseSandboxSeriveIsDisabled()
        {
            var mockRequestContext = new Mock<IRequestContext>();
            var mockProvideSettings = new Mock<IProvideSettings>();
            mockProvideSettings.Setup(ps => ps.GetNullableSetting(It.IsAny<string>())).Returns((string)null);
            var container = new Container(new InfrastructureRegistry(mockProvideSettings.Object));

            container.Configure(c =>
            {
                c.For<IRequestContext>().Use(mockRequestContext.Object);
            });

            Assert.That(container.GetInstance<IProvideSettings>(),
                Is.SameAs(mockProvideSettings.Object));

            Assert.That(container.GetInstance<ICreateApprenticeshipService>(),
                Is.InstanceOf<CreateApprenticeshipService>());
        }

    }
}